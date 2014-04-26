float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float xAmbient;
bool xEnableLighting;
float xTime2;
float3 xLightPos;
float xLightPower;

Texture xTexture;
Texture xTexture0; 
Texture xTexture1; 
Texture xTexture2;
Texture xTexture3;
Texture xTexture4;
Texture xTexture5;
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; }; 

sampler TextureSampler0 = sampler_state { texture = <xTexture0>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

sampler TextureSampler1 = sampler_state { texture = <xTexture1>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

sampler TextureSampler2 = sampler_state { texture = <xTexture2>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; }; 

sampler TextureSampler3 = sampler_state { texture = <xTexture3>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };
sampler TextureSampler4 = sampler_state { texture = <xTexture4>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };
sampler TextureSampler5 = sampler_state { texture = <xTexture5>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

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


	return Output;
}

MTPixelToFrame MultiTexturedPS(MTVertexToPixel PSIn)
{
	
	MTPixelToFrame Output = (MTPixelToFrame)0;

	float diffuseLightingFactor;
	if (xEnableLighting)
	{
		
		diffuseLightingFactor = DotProduct((xLightPos.x*sin(radians(xTime2)), abs(xLightPos.y*sin(radians(xTime2))), xLightPos.z*sin(radians(xTime2))), PSIn.Position3D, PSIn.Normal);
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
	float2 nearTextureCoords = PSIn.TextureCoords * 3;
	nearColor = tex2D(TextureSampler0, nearTextureCoords)*PSIn.TextureWeights.x;
	nearColor += tex2D(TextureSampler1, nearTextureCoords)*PSIn.TextureWeights.y;
	nearColor += tex2D(TextureSampler2, nearTextureCoords)*PSIn.TextureWeights.z;
	nearColor += tex2D(TextureSampler3, nearTextureCoords)*PSIn.TextureWeights.w;

	Output.Color = lerp(nearColor, farColor, blendFactor)*(diffuseLightingFactor + xAmbient);


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