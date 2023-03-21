using MassTransit;
using Prototype_MassTransit.Contracts.Reserves;

namespace Prototype_MassTransit.Contracts;

//Todo: Need to dig deeper on this, see https://www.youtube.com/watch?v=T-QUVy7qrRk&list=PLx8uyNNs1ri1UA_Nerr7Ej3g9nT2PxbbH&index=1
//Apparently needed for tests
public static class MessageContracts
{
    private static bool _initialized;

    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        GlobalTopology.Send.UseCorrelationId<ISubmitReserve>(x => x.Id);
        GlobalTopology.Send.UseCorrelationId<ISubmittedReserve>(x => x.Id);

        _initialized = true;
    }
}