using System.Threading;
using System.Threading.Tasks;
using EventStore.Client.Projections;

#nullable enable
namespace EventStore.Client {
	public partial class EventStoreProjectionManagementClient {
		/// <summary>
		/// Enables a projection.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="userCredentials"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task EnableAsync(string name, UserCredentials? userCredentials = null,
			CancellationToken cancellationToken = default) {
			using var call = _client.EnableAsync(new EnableReq {
				Options = new EnableReq.Types.Options {
					Name = name
				}
			}, EventStoreCallOptions.Create(Settings, Settings.OperationOptions, userCredentials, cancellationToken));
			await call.ResponseAsync.ConfigureAwait(false);
		}

		/// <summary>
		/// Aborts a projection. Saves the projection's checkpoint.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="userCredentials"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task AbortAsync(string name, UserCredentials? userCredentials = null,
			CancellationToken cancellationToken = default) =>
			DisableInternalAsync(name, true, userCredentials, cancellationToken);

		/// <summary>
		/// Disables a projection. Does not save the projection's checkpoint.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="userCredentials"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task DisableAsync(string name, UserCredentials? userCredentials = null,
			CancellationToken cancellationToken = default) =>
			DisableInternalAsync(name, false, userCredentials, cancellationToken);

		private async Task DisableInternalAsync(string name, bool writeCheckpoint, UserCredentials? userCredentials,
			CancellationToken cancellationToken) {
			using var call = _client.DisableAsync(new DisableReq {
				Options = new DisableReq.Types.Options {
					Name = name,
					WriteCheckpoint = writeCheckpoint
				}
			}, EventStoreCallOptions.Create(Settings, Settings.OperationOptions, userCredentials, cancellationToken));
			await call.ResponseAsync.ConfigureAwait(false);
		}

		/// <summary>
		/// Restarts the projection subsystem.
		/// </summary>
		/// <param name="userCredentials"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task RestartSubsystemAsync(UserCredentials? userCredentials = null,
			CancellationToken cancellationToken = default) {
			await _client.RestartSubsystemAsync(new Empty(),
				EventStoreCallOptions.Create(Settings, Settings.OperationOptions, userCredentials, cancellationToken));
		}
	}
}
