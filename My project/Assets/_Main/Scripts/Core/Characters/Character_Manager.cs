using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Manager : MonoBehaviour
    {
        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfigurationAsset;    
        public static Character_Manager instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();
        private void Awake()
        {
            instance = this;
        }

        public Character GetCharacter(string characterName, bool createIfDoesNotExist = false)
        {
            if(characters.ContainsKey(characterName.ToLower()))
                return characters[characterName.ToLower()];
            else if(createIfDoesNotExist)
                return CreateCharacter(characterName);

            return null;
        }

        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }

        public Character CreateCharacter(string characterName)
        {
            if(characters.ContainsKey(characterName.ToLower()))
            {
                Debug.Log("El personaje ya existe");
                return null;
            }

            CHARACTER_INFO info = GetCharacterInfo(characterName);

            Character character = CreateCharacterFromInfo(info);

            characters.Add(characterName.ToLower(), character);

            return character;
        }

        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new CHARACTER_INFO();
            result.name = characterName;

            result.config = config.GetConfig(characterName);

            return result;
        }

        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            CharacterConfigData config = info.config;

            if(config.characterType == Character.CharacterType.Text)
                return new Character_Text(info.name, config);
            if(config.characterType == Character.CharacterType.Sprite || config.characterType == Character.CharacterType.SpriteSheet)
                return new Character_Sprite(info.name, config);
            if(config.characterType == Character.CharacterType.Live2D)
                return new Character_Live2D(info.name, config);
            if(config.characterType == Character.CharacterType.Model3d)
                return new Character_3DModel(info.name, config);

            return null;
        }   

        private class CHARACTER_INFO
        {
            public string name= "";
            public CharacterConfigData config = null;
        }
    }
}