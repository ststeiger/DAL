
using System;
using System.Collections.Generic;


namespace Subtext.Scripting
{
    

    public class TemplateParameter
    {
        string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="defaultValue">The default value.</param>
        public TemplateParameter(string name, string type, string defaultValue)
        {
            Name = name;
            DataType = type;
            _value = defaultValue;
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public string DataType { get; private set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    //OnValueChanged(_value, value);
                }
                _value = value;
            }
        }

    }


    public class Script
    {
        public string ScriptText
        {
            get {
                return "";
            }
            set { }
        }
    }


    public class SqlScriptRunner
    {


        public SqlScriptRunner()
        {}

        public SqlScriptRunner(string str)
        {}

        public List<Script> ScriptCollection  = new List<Script>();

        protected Dictionary<string, object> my = new Dictionary<string, object>();

        protected Dictionary<string, TemplateParameter> xxx = new Dictionary<string, TemplateParameter>();

        public Dictionary<string,TemplateParameter> TemplateParameters
        {
            get { return xxx; }
        }


    }


}
