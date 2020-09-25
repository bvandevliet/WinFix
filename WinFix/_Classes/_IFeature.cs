interface _IFeature
{
    string Name
    {
        get;
    }
    string Description
    {
        get;
    }

    bool Default
    {
        get;
    }
    dynamic Recommended
    {
        get;
    }
    bool Optimized
    {
        get;
    }

    bool Enabled
    {
        get;
        //set;
    }

    void Enable(bool Enable);
}
