using Kitchen;
using KitchenMods;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace AdvancedUIReboot
{
    public class UISystem : GenericSystemBase, IModSystem
    {
        private EntityQuery _coinDisplayQuery;
        private EntityQuery _groupDisplayQuery;
        private EntityQuery _menuDisplayQuery;
        private EntityQuery _timeDisplayQuery;
        private EntityQuery _dayDisplayQuery;
        private EntityQuery _playerListDisplayQuery;
        private EntityQuery _RequiresViewDisplayQuery;

        private Dictionary<UIElement, EntityQuery> keyValuePairs;

        protected override void Initialise()
        {
            _coinDisplayQuery = GetEntityQuery(new QueryHelper().All(typeof(CMoneyDisplay)));
            _groupDisplayQuery = GetEntityQuery(new QueryHelper().All(typeof(CParametersDisplay)));
            _menuDisplayQuery = GetEntityQuery(new QueryHelper().All(typeof(SPlayerInfoManager)));
            _timeDisplayQuery = GetEntityQuery(new QueryHelper().All(typeof(CTimeDisplay)));
            _dayDisplayQuery = GetEntityQuery(new QueryHelper().All(typeof(CDayDisplay)));
            _playerListDisplayQuery = GetEntityQuery(new QueryHelper().All(typeof(CTwitchOrderOption)));
            _RequiresViewDisplayQuery = GetEntityQuery(new QueryHelper().All(typeof(CRequiresView)));

            keyValuePairs = new Dictionary<UIElement, EntityQuery>
            {
                { UIElement.Coin, _coinDisplayQuery},
                { UIElement.Group, _groupDisplayQuery},
                { UIElement.Menu, _menuDisplayQuery},
                { UIElement.Time, _timeDisplayQuery},
                { UIElement.Day, _dayDisplayQuery},
                { UIElement.PlayerList, _playerListDisplayQuery},
                { UIElement.VersionInfo, _RequiresViewDisplayQuery }
            };
        }

        protected override void OnUpdate()
        {
            foreach (UIElement element in PreferenceSystemManagerExtensions._elements)
            {
                if (Mod.Controller.MenuBehaviourDictionary.TryGetValue(element, out MenuBehavior behavior))
                {
                    if (FindEntity(element, keyValuePairs[element]) is Entity entity && entity != Entity.Null)
                    {
                        CPosition pos = EntityManager.GetComponentData<CPosition>(entity);
                        if (!behavior.initialized &&
                            pos.Position != new Vector3(0.5f, 0.5f, 0))
                        {
                            behavior.InitialPos = pos.Position;
                            behavior.draggable = Mod.Controller.GetElementPosition(element);
                            behavior.visibility = Mod.Controller.GetElementVisibility(element);

                            if (!Mod.Controller.PreferenceHasValue(element))
                            {
                                Mod.Controller.SetPreferenceValue(element, pos);
                            }
                            else
                            {
                                EntityManager.SetComponentData(entity, new CPosition((Vector3)Mod.Controller.GetPreferenceValue(element)));
                            }
                            behavior.initialized = true;
                        }

                        if (behavior.initialized)
                        {
                            if (behavior.visibility_changed)
                            {
                                if (behavior.visibility)
                                {
                                    EntityManager.SetComponentData(entity, CPosition.Hidden);
                                }
                                else
                                {
                                    EntityManager.SetComponentData(entity, new CPosition((Vector3)Mod.Controller.GetPreferenceValue(element)));
                                }
                                behavior.visibility_changed = false;
                            }

                            if (behavior.dragging)
                            {
                                Vector3 vector = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                                EntityManager.SetComponentData(entity, new CPosition(vector));

                                if (Input.anyKeyDown)
                                {
                                    behavior.dragging = false;
                                    Mod.Controller.SetPreferenceValue(element, vector);
                                    Mod.Controller.SavePreference();
                                }
                            }
                        }
                    }
                }
            }
        }

        public Entity? FindEntity(UIElement element, EntityQuery query)
        {
            if (element == UIElement.VersionInfo)
            {
                return query
                    .ToEntityArray(Allocator.Temp)
                    .Where(_ => EntityManager.GetComponentData<CRequiresView>(_).Type == ViewType.VersionOverlay)
                    .FirstOrDefault();
            }
            return query
                .ToEntityArray(Allocator.Temp)
                .FirstOrDefault();
        }
    }
}
