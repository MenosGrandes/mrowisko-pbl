//----------------------------------------------------
//-- --
//-- www.riemers.net --
//-- Series 4: Advanced terrain --
//-- Shader code --
//-- --
//----------------------------------------------------

//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xLightDirection;
float3 xCamPos;
float3 xAllowedRotDir;
int scale;
float xAmbient;
bool xEnableLighting;

//------- Texture Samplers --------
Texture xTexture;
Texture xBillboardTexture;

sampler TextureSampler = sampler_state { texture = <xTexture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};Texture xTexture0;

sampler TextureSampler0 = sampler_state { texture = <xTexture0> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};Texture xTexture1;

sampler TextureSampler1 = sampler_state { texture = <xTexture1> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};Texture xTexture2;

sampler TextureSampler2 = sampler_state { texture = <xTexture2> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};Texture xTexture3;

sampler TextureSampler3 = sampler_state { texture = <xTexture3> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

sampler textureSampler = sampler_state { texture = <xBillboardTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

//------- Technique: Textured --------
struct TVertexToPixel
{
float4 Position     : POSITION;
float4 Color        : COLOR0;
float LightingFactor: TEXCOORD0;
float2 TextureCoords: TEXCOORD1;
};
//------- Technique: Bilboarding --------
struct TPixelToFrame
{
float4 Color : COLOR0;
};

struct BBVertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoord	: TEXCOORD0;
};
struct BBPixelToFrame
{
	float4 Color 	: COLOR0;
};



TVertexToPixel TexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{
    TVertexToPixel Output = (TVertexToPixel)0;
    float4x4 preViewProjection = mul (xView, xProjection);
    float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);

    Output.Position = mul(inPos, preWorldViewProjection);
    Output.TextureCoords = inTexCoords;

    float3 Normal = normalize(mul(normalize(inNormal), xWorld));
    Output.LightingFactor = 1;
    if (xEnableLighting)
        Output.LightingFactor = saturate(dot(Normal, -xLightDirection));

    return Output;
}

TPixelToFrame TexturedPS(TVertexToPixel PSIn)
{
    TPixelToFrame Output = (TPixelToFrame)0;

    Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
    Output.Color.rgb *= saturate(PSIn.LightingFactor + xAmbient);

    return Output;
}

technique Textured_2_0
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 TexturedVS();
        PixelShader = compile ps_2_0 TexturedPS();
    }
}

technique Textured
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 TexturedVS();
        PixelShader = compile ps_2_0 TexturedPS();
    }
}

//------- Technique: Multitextured --------
struct MTVertexToPixel
{
    float4 Position         : POSITION;
    float4 Color            : COLOR0;
    float3 Normal            : TEXCOORD0;
    float2 TextureCoords    : TEXCOORD1;
    float4 LightDirection    : TEXCOORD2;
    float4 TextureWeights    : TEXCOORD3;

     float Depth                : TEXCOORD4;

};

struct MTPixelToFrame
{
    float4 Color : COLOR0;
};

MTVertexToPixel MultiTexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0, float4 inTexWeights: TEXCOORD1)
{    
    MTVertexToPixel Output = (MTVertexToPixel)0;
    float4x4 preViewProjection = mul (xView, xProjection);
    float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);

    Output.Position = mul(inPos, preWorldViewProjection);
    Output.Normal = mul(normalize(inNormal), xWorld);
    Output.TextureCoords = inTexCoords;
    Output.LightDirection.xyz = -xLightDirection;
    Output.LightDirection.w = 1;    
    Output.TextureWeights = inTexWeights;

     Output.Depth = Output.Position.z/Output.Position.w;


    return Output;
}

MTPixelToFrame MultiTexturedPS(MTVertexToPixel PSIn)
{
    MTPixelToFrame Output = (MTPixelToFrame)0;        
    
    float lightingFactor = 1;
    if (xEnableLighting)
        lightingFactor = saturate(saturate(dot(PSIn.Normal, PSIn.LightDirection)) + xAmbient);

         
     float blendDistance = 0.99f;
     float blendWidth = 0.005f;
     float blendFactor = clamp((PSIn.Depth-blendDistance)/blendWidth, 0, 1);
         
     float4 farColor;
     farColor = tex2D(TextureSampler0, PSIn.TextureCoords)*PSIn.TextureWeights.x;
     farColor += tex2D(TextureSampler1, PSIn.TextureCoords)*PSIn.TextureWeights.y;
     farColor += tex2D(TextureSampler2, PSIn.TextureCoords)*PSIn.TextureWeights.z;
     farColor += tex2D(TextureSampler3, PSIn.TextureCoords)*PSIn.TextureWeights.w;
     
     float4 nearColor;
     float2 nearTextureCoords = PSIn.TextureCoords*3;
     nearColor = tex2D(TextureSampler0, nearTextureCoords)*PSIn.TextureWeights.x;
     nearColor += tex2D(TextureSampler1, nearTextureCoords)*PSIn.TextureWeights.y;
     nearColor += tex2D(TextureSampler2, nearTextureCoords)*PSIn.TextureWeights.z;
     nearColor += tex2D(TextureSampler3, nearTextureCoords)*PSIn.TextureWeights.w;
 
     Output.Color = lerp(nearColor, farColor, blendFactor);
     Output.Color *= lightingFactor;

    
    return Output;
}

technique MultiTextured
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 MultiTexturedVS();
        PixelShader = compile ps_2_0 MultiTexturedPS();
    }
}

//------- Technique: CylBillboard --------
BBVertexToPixel CylBillboardVS(float3 inPos: POSITION0, float2 inTexCoord : TEXCOORD0)
{
	BBVertexToPixel Output = (BBVertexToPixel)0;

	float3 center = mul(inPos, xWorld);
		float3 eyeVector = center - xCamPos;
		int scaling = scale;
		float3 upVector = xAllowedRotDir;
		upVector = normalize(upVector);
	float3 sideVector = cross(eyeVector, upVector);
		sideVector = normalize(sideVector);

	float3 finalPosition = center;
		finalPosition += (inTexCoord.x - 0.5f)*sideVector*scale;
	finalPosition += (1.5f - inTexCoord.y*1.5f)*upVector*scale;

	float4 finalPosition4 = float4(finalPosition, 1);

		float4x4 preViewProjection = mul(xView, xProjection);
		Output.Position = mul(finalPosition4, preViewProjection);

	Output.TexCoord = inTexCoord;

	return Output;
}

BBPixelToFrame BillboardPS(BBVertexToPixel PSIn) : COLOR0
{
	BBPixelToFrame Output = (BBPixelToFrame)0;
	Output.Color = tex2D(textureSampler, PSIn.TexCoord);

	clip(Output.Color.w - 0.7843f);

	return Output;
}

technique CylBillboard
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 CylBillboardVS();
		PixelShader = compile ps_2_0 BillboardPS();
	}
}