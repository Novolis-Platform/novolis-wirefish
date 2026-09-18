namespace Frank.WireFish;

/// <summary>
/// Handles captured <see cref="DevicePacket"/> instances when <see cref="CanHandle"/> returns true.
/// </summary>
public interface IPacketHandler
{
    /// <summary>
    /// Processes a packet asynchronously.
    /// </summary>
    /// <param name="packet">Captured device packet.</param>
    /// <param name="cancellationToken">Token used to cancel handling.</param>
    /// <returns>A task that completes when handling finishes.</returns>
    Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether this handler should process the packet.
    /// </summary>
    /// <param name="packet">Captured device packet.</param>
    /// <returns><see langword="true"/> when <see cref="HandleAsync"/> should run.</returns>
    bool CanHandle(DevicePacket packet);
}
