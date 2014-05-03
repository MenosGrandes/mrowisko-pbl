float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float xAmbient;
bool xEnableLighting;
//float xTime2;
float3 xLightPos;
float xLightPower;
float4x4 xLightsWorldViewProjection;
float4x4 xWorldViewProjection;


Texture xTexture;
Texture xTexture0; 
Texture xTexture1; 
Texture xTexture2;
Texture xTexture3;
Texture xTexture4;
Texture xTexture5;
Texture xShadowMap;
texture Ground;


sampler ShadowMapSampler = sampler_state { texture = <xShadowMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = clamp; AddressV = clamp; };


sampler TextureSampler = sampler_state 
{ texture = <xTexture>; 
magfilter = LINEAR;
minfilter = LINEAR; 
mipfilter = LINEAR; 
AddressU = wrap; 
AddressV = wrap; }; 

sampler TextureSampler0 = sampler_state
{ texture = <xTexture0>; 
magfilter = LINEAR; 
minfilter = LINEAR;
mipfilter = LINEAR;
AddressU = wrap;
AddressV = wrap; };

sampler TextureSampler1 = sampler_state 
{ texture = <xTexture1>; 
magfilter = LINEAR; 
minfilter = LINEAR; 
mipfilter = LINEAR; 
AddressU = wrap;
AddressV = wrap; };

sampler TextureSampler2 = sampler_state
{ texture = <xTexture2>; 
magfilter = LINEAR; 
minfilter = LINEAR;
mipfilter = LINEAR;
AddressU = wrap;
AddressV = wrap; }; 

sampler TextureSampler3 = sampler_state
{ texture = <xTexture3>;
magfilter = LINEAR;
minfilter = LINEAR; 
mipfilter = LINEAR;
AddressU = wrap;
AddressV = wrap; };
sampler TextureSampler4 = sampler_state 
{ texture = <xTexture4>; 
magfilter = LINEAR; 
minfilter = LINEAR;
mipfilter = LINEAR; 
AddressU = wrap; 
AddressV = wrap; };
sampler TextureSampler5 = sampler_state 
{ texture = <xTexture5>;
magfilter = LINEAR; 
minfilter = LINEAR;
mipfilter = LINEAR;
AddressU = wrap;
AddressV = wrap; };
sampler GroundSampler = sampler_state
{
	Texture = (Ground);

	MinFilter = LINEAR;
	MagFilter = LINEAR;

	AddressU = clamp;
	AddressV = clamp;
};


float GroundText0Scale;
float GroundText1Scale;
float GroundText2Scale;

texture GroundText0;
sampler GroundText0Sampler = sampler_state
{
	Texture = (GroundText0);

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = wrap;
	AddressV = wrap;
};

texture GroundText1;
sampler GroundText1Sampler = sampler_state
{
	Texture = (GroundText1);

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = wrap;
	AddressV = wrap;
};

texture GroundText2;
sampler GroundText2Sampler = sampler_state
{
	Texture = (GroundText2);

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = wrap;
	AddressV = wrap;
};
//------- Technique: Multitextured --------
struct MTVertexToPixel
{
	float4 Position         : POSITION;
	float4 Color            : COLOR0;
	float3 Normal            : TEXCOORD0;
	float2 TextureCoords    : TEXCOORD1;
	float4 LightDirection    : TEXCOORD2;
	float4 TextureWeights    : TEXCOORD3;


	float Depth : TEXCOORD4;
	float4 clipDistances                : TEXCOORD5;
	float4 Position3D        : TEXCOORD6;

};

struct MTPixelToFrame
{
	float4 Color : COLOR0;
};

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
	float3 lightDir = normalize(pos3D - lightPos);
		return dot(-lightDir, normal);
}

MTVertexToPixel MultiTexturedVS(float4 inPos : POSITION, float3 inNormal : NORMAL, float2 inTexCoords : TEXCOORD0, float4 inTexWeights : TEXCOORD1)
{
	MTVertexToPixel Output = (MTVertexToPixel)0;
	float4x4 preViewProjection = mul(xView, xProjection);
		float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);

		Output.Position = mul(inPos, preWorldViewProjection);
	Output.Normal = mul(normalize(inNormal), xWorld);
	Output.TextureCoords = inTexCoords;
	//Output.LightDirection.xyz = -xLightDirection;
	//Output.LightDirection.w = 1;    
	Output.TextureWeights = inTexWeights;
	Output.Position3D = mul(inPos, xWorld);

	Output.Depth = Output.Position.z / Output.Position.w;
	//Output.clipDistances = dot(inPos, ClipPlane0); //MSS - Water Refactor added


	return Output;
}

MTPixelToFrame MultiTexturedPS(MTVertexToPixel PSIn)
{
	
	MTPixelToFrame Output = (MTPixelToFrame)0;

	float diffuseLightingFactor=0.3f;
	if (xEnableLighting)
	{
		
		diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
		diffuseLightingFactor = saturate(diffuseLightingFactor);
		diffuseLightingFactor *= xLightPower;
	}

	float blendDistance = 0.99f;
	float blendWidth = 0.005f;
	float blendFactor = clamp((PSIn.Depth - blendDistance) / blendWidth, 0, 1);

	float4 farColor;
	farColor = tex2D(TextureSampler0, PSIn.TextureCoords)*PSIn.TextureWeights.x;
	farColor += tex2D(TextureSampler1, PSIn.TextureCoords)*PSIn.TextureWeights.y;
	farColor += tex2D(TextureSampler2, PSIn.TextureCoords)*PSIn.TextureWeights.z;
	farColor += tex2D(TextureSampler3, PSIn.TextureCoords)*PSIn.TextureWeights.w;

	float4 nearColor;
	float2 nearTextureCoords = PSIn.TextureCoords /3;
	nearColor = tex2D(TextureSampler0, nearTextureCoords)*PSIn.TextureWeights.x;
	nearColor += tex2D(TextureSampler1, nearTextureCoords)*PSIn.TextureWeights.y;
	nearColor += tex2D(TextureSampler2, nearTextureCoords)*PSIn.TextureWeights.z;
	nearColor += tex2D(TextureSampler3, nearTextureCoords)*PSIn.TextureWeights.w;

	float4 groundSample = tex2D(GroundSampler, PSIn.TextureCoords / 100);

		float4 colour = float4(0, 0, 0, 0);
	colour += tex2D(GroundText0Sampler, PSIn.TextureCoords*10) *groundSample.r;
	colour += tex2D(GroundText1Sampler, PSIn.TextureCoords) * groundSample.g;
	colour += tex2D(GroundText2Sampler, PSIn.TextureCoords) * groundSample.b;
	
	//Output.Color = ;
	Output.Color += lerp(nearColor, farColor, blendWidth)*(diffuseLightingFactor + xAmbient);
	Output.Color += colour;

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



struct SMapVertexToPixel
{
	float4 Position     : POSITION;
	float4 Position2D    : TEXCOORD0;
};

struct SMapPixelToFrame
{
	float4 Color : COLOR0;
};


struct SSceneVertexToPixel
{
	float4 Position             : POSITION;
	float4 Pos2DAsSeenByLight    : TEXCOORD0;

	float2 TexCoords            : TEXCOORD1;
	float3 Normal                : TEXCOORD2;
	float4 Position3D            : TEXCOORD3;

};

struct SScenePixelToFrame
{
	float4 Color : COLOR0;
};

SMapVertexToPixel ShadowMapVertexShader(float4 inPos : POSITION)
{
	SMapVertexToPixel Output = (SMapVertexToPixel)0;

	Output.Position = mul(inPos, xLightsWorldViewProjection);
	Output.Position2D = Output.Position;

	return Output;
}

SMapPixelToFrame ShadowMapPixelShader(SMapVertexToPixel PSIn)
{
	SMapPixelToFrame Output = (SMapPixelToFrame)0;

	Output.Color = PSIn.Position2D.z / PSIn.Position2D.w;

	return Output;
}


technique ShadowMap
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ShadowMapVertexShader();
		PixelShader = compile ps_2_0 ShadowMapPixelShader();
	}
}

SSceneVertexToPixel ShadowedSceneVertexShader(float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0, float3 inNormal : NORMAL)
{
	SSceneVertexToPixel Output = (SSceneVertexToPixel)0;

	Output.Position = mul(inPos, xWorldViewProjection);
	Output.Pos2DAsSeenByLight = mul(inPos, xLightsWorldViewProjection);
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);
	Output.TexCoords = inTexCoords;

	return Output;
}

SScenePixelToFrame ShadowedScenePixelShader(SSceneVertexToPixel PSIn)
{
	SScenePixelToFrame Output = (SScenePixelToFrame)0;

	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = PSIn.Pos2DAsSeenByLight.x / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;
	ProjectedTexCoords[1] = -PSIn.Pos2DAsSeenByLight.y / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;

	float diffuseLightingFactor = 0;
	if ((saturate(ProjectedTexCoords).x == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords).y == ProjectedTexCoords.y))
	{
		float depthStoredInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords).r;
		float realDistance = PSIn.Pos2DAsSeenByLight.z / PSIn.Pos2DAsSeenByLight.w;
		if ((realDistance - 1.0f / 100.0f) <= depthStoredInShadowMap)
		{
			diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
			diffuseLightingFactor = saturate(diffuseLightingFactor);
			diffuseLightingFactor *= xLightPower;
		}
	}

	float4 baseColor = tex2D(GroundText2Sampler, PSIn.TexCoords);	//TUTAJ TRZEBA ZMIENIC!!! NA MULTITEXTURE!
		Output.Color = baseColor*(diffuseLightingFactor + xAmbient);

	return Output;
}


technique ShadowedScene
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ShadowedSceneVertexShader();
		PixelShader = compile ps_2_0 ShadowedScenePixelShader();
	}
}
