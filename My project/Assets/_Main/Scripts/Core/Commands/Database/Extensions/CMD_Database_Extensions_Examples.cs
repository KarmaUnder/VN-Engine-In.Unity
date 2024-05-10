using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COMMANDS;



namespace TESTING
{

    public class CMD_Database_Extensions_Examples : CMD_Database_Extension
    {
        new public static void Extend(Command_Database database)
        {
            //Add command with no parameters
            database.AddCommand("print", new Action(PrintDefaultMessage));
            database.AddCommand("print_1p", new Action<string>(PrintUserMessage));
            database.AddCommand("print_mp", new Action<string[]>(PrintLines));

            //Add lambda with no parameters
            database.AddCommand("Lambda", new Action(() => { Debug.Log("Printing a default Message to console from lambda command"); }));
            database.AddCommand("Lambda_1p", new Action<string>((arg) => { Debug.Log($"Log User Lambda message: '{arg}'"); }));
            database.AddCommand("Lambda_mp", new Action<string[]>((args) => { Debug.Log(string.Join(", ", args)); }));

            //Add Coroutines with no parameters
            database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
            database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
            database.AddCommand("process_mp", new Func<string[], IEnumerator>(MultiLineProcess));

            // special Example
            database.AddCommand("moveCharDemo", new Func<string, IEnumerator>(MoveCharacter));

        }

        private static void PrintDefaultMessage()
        {
            Debug.Log("Printing a default Message to console");
        }

        private static void PrintUserMessage(string message)
        {
            Debug.Log($"User Message: '{message}'");
        }

        private static void PrintLines(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"{i++}. '{line}'");
            }
        }

        private static IEnumerator SimpleProcess()
        {
            for (int i = 0; i <= 5; i++)
            {
                Debug.Log($"Process Running... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }

        private static IEnumerator LineProcess(string data)
        {
            if (int.TryParse(data, out int num))
            {
                for (int i = 0; i <= num; i++)
                {
                    Debug.Log($"Process Running... [{i}]");
                    yield return new WaitForSeconds(1);
                }

            }
        }

        private static IEnumerator MultiLineProcess(string[] data)
        {
            foreach (string line in data)
            {
                Debug.Log($"Process Message: '{line}'");
                yield return new WaitForSeconds(0.5f);
            }
        }

        private static IEnumerator MoveCharacter(string direction)
        {
            bool left = direction.ToLower() == "left";

            Transform character = GameObject.Find("Image").transform;
            float moveSpeed = 15;

            float targetX = left ? -8 : 8;
            float currentX = character.position.x;

            while (Mathf.Abs(targetX - currentX) > 0.1f)
            {
                currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
                character.position = new Vector3(currentX, character.position.y, character.position.z);
                yield return null;
            }
        }
    }
}