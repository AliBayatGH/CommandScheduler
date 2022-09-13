using System.Linq;

namespace CommandScheduler
{
    public class MediatorSerializedObject
    {
        /// <summary>
        /// Create an instance of MediatorSerializedObject
        /// </summary>
        /// <param name="fullTypeName"></param>
        /// <param name="data"></param>
        /// <param name="additionalDescription"></param>
        /// <param name="assemblyName"></param>
        public MediatorSerializedObject(string fullTypeName, string data, string additionalDescription, string assemblyName = "")
        {
            FullTypeName = fullTypeName;
            Data = data;
            AdditionalDescription = additionalDescription;
            AssemblyName = assemblyName == "" ? "Application.Commands" : assemblyName;
        }

        public string FullTypeName { get; private set; }
        public string Data { get; private set; }
        public string AdditionalDescription { get; private set; }
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Override for Hangfire dashboard display.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var commandName = FullTypeName.Split('.').Last();
            return $"{commandName} {AdditionalDescription}";
        }
    }
}
