namespace LabelRecognizer
{
    public interface ILabelNameLookupTable
    {
        bool IncludesTarget(TargetKey target, string name, LookupLanguages language);
    }
}