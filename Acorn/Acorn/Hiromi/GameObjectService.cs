﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Acorn.Hiromi
{
    public class GameObjectService
    {
        private static GameObjectService _instance;
        public static GameObjectService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObjectService();
                }
                return _instance;
            }
        }

        private int _nextObjectId = 0;
        private List<GameObject> _objects;

        private GameObjectService()
        {
            _objects = new List<GameObject>();
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObject.Id = _nextObjectId;
            _nextObjectId++;
            _objects.Add(gameObject);
        }

        public List<GameObject> GetAllGameObjects()
        {
            return _objects;
        }
    }
}