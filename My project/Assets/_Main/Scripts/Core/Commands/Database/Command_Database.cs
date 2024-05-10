using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace COMMANDS
{

    public class Command_Database
    {
        private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

        public bool hasCommand(string commandName) => database.ContainsKey(commandName);

        public void AddCommand(string commandName, Delegate command)
        {
            if (!database.ContainsKey(commandName))
            {
                database.Add(commandName, command);
            }
            else
                Debug.Log("El comando ya existe en la base de datos.");
        }

        public Delegate GetCommand(string commandName)
        {
            if (!database.ContainsKey(commandName))
            {
                Debug.Log("El comando no existe en la base de datos.");
                return null;
            }

            return database[commandName];
        }
    }
}