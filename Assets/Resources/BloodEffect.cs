using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(BloodEffectRenderer), PostProcessEvent.AfterStack, "Custom/BloodEffect")]
public sealed class BloodEffect : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("BloodEffect effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
    public FloatParameter intensity = new FloatParameter { value = 0.5f };
    public ColorParameter effectColor = new ColorParameter { value = Color.white };
}

public sealed class BloodEffectRenderer : PostProcessEffectRenderer<BloodEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/BloodEffect"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetColor("_Intensity", Color.HSVToRGB(0, 0, settings.intensity));
        sheet.properties.SetColor("_EffectColor", settings.effectColor);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}