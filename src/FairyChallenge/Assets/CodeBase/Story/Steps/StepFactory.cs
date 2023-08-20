using System;
using System.Collections.Generic;

namespace Fairy
{
    public sealed class StepFactory
    {
        private readonly List<IConcreteStepFactory> _concreteStepFactories;

        public StepFactory(List<IConcreteStepFactory> concreteStepFactories)
        {
            _concreteStepFactories = concreteStepFactories;
        }

        public IStep Create(StepStaticData stepStaticData)
        {
            foreach (var concreteNodeFactory in _concreteStepFactories)
            {
                if (concreteNodeFactory.Type == stepStaticData.Type)
                {
                    return concreteNodeFactory.Create(stepStaticData);
                }
            }

            throw new Exception($"Can't find concrete factory for '{stepStaticData.Type}'");
        }
    }
}