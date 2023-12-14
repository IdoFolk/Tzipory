namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityExperienceComponent : IEntityComponent
    {
        public int EntityLevel { get; }
        public float EntityExperience { get; }

        public void AddExperience(float experience);
    }
}