using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.Systems.AIWeightSystem
{
    /// <summary>
    /// Not the only type of Actor -> but the most common one.
    /// Simpler/different ones may be needed in the future, so it wont be the ONLY implementation of IActor
    /// </summary>
    public class BasicCharacter : MonoBehaviour, IActor
    {
        [SerializeField, Tooltip("This list should be filled in the Perfab _levelToOpen. Use context-method 'Re-get Action Components' to make sure.")]
        ActionComponent[] actionComponents;//may need to be a list

        List<ActionVariation> actionVariations;

        public void CalculateActionVariations()
        {
            actionVariations = new List<ActionVariation>();

            foreach (var ac in actionComponents)
            {
                actionVariations.AddRange(ac.CalculateVariations());
            }
        }

        public List<ActionVariation> GetActionVariations() => actionVariations;

        public void PerformAction()
        {
            throw new System.NotImplementedException();
        }

        [ContextMenu("Re-get Action Components")] //for inspector use only!
        public void GetActionComponents()
        {
            actionComponents = GetComponents<ActionComponent>();
        }
    }

}

