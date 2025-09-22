using Assets.Scripts.Enums;
using Assets.Scripts.LocationLogic;
using Assets.Scripts.Shops;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.BootstrapperS
{
    public class BootstrapperSceneGame : Bootstrapper
    {
        [SerializeField] private LocationCreate _locationCreate;
        [SerializeField] private ChangeTemplate _changeTemplateBall;
        [SerializeField] private ChangeTemplate _changeTemplatePlatform;
        [SerializeField] private Counter _counter;

        private void Update()
        {
            _counter.UpdateTime();
        }

        protected override void OnInit()
        {
            _locationCreate.Init(SaveService.LevelData);
            _changeTemplateBall.EnableCurrentTemplate(SaveService.GetCurrentProduct(ObjectsName.Ball),
                                                      SaveService.GetScale(ObjectsName.Ball));

            _changeTemplatePlatform.EnableCurrentTemplate(SaveService.GetCurrentProduct(ObjectsName.Platform),
                                                          SaveService.GetScale(ObjectsName.Platform));

            SaveService.SaveScale(false, ObjectsName.Ball);
            SaveService.SaveScale(false, ObjectsName.Platform);
        }
    }
}