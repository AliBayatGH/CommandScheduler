using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CommandSchedular
{
    public class CommandsExecutor
    {
        private readonly IMediator mediator;
        public CommandsExecutor(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [DisplayName("Processing command {0}")]
        public Task ExecuteCommand(MediatorSerializedObject mediatorSerializedObject)
        {
            var type = GetType(mediatorSerializedObject);

            if (type != null)
            {
                dynamic req = JsonConvert.DeserializeObject(mediatorSerializedObject.Data, type);

                return mediator.Send(req as IRequest);
            }

            return null;
        }

        public Task ExecuteCommand(string reuest)
        {
            MediatorSerializedObject mediatorSerializedObject = JsonConvert.DeserializeObject<MediatorSerializedObject>(reuest);

            return ExecuteCommand(mediatorSerializedObject);
        }

        private System.Type GetType(MediatorSerializedObject mediatorSerializedObject)
        {
            var assemblies = GetAssemblies().ToList()
               .Where(x => x.GetName().Name.EndsWith("Application.Commands"))
               .Select(x => x.GetName().FullName)
               .ToList();

            var type = assemblies.SelectMany(x => Assembly.Load(x)
                .GetTypes()
                .Where(t => t.FullName == mediatorSerializedObject.FullTypeName)
                .ToList()).First();
            return type;
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            var list = new List<string>();
            var stack = new Stack<Assembly>();

            stack.Push(Assembly.GetEntryAssembly());

            do
            {
                var asm = stack.Pop();

                yield return asm;

                foreach (var reference in asm.GetReferencedAssemblies())
                {
                    if (reference.Name.StartsWith("HSP") && !list.Contains(reference.FullName))
                    {
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }
                }
            }
            while (stack.Count > 0);

        }

    }
}
