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

bool Clipping;
float4 ClipPlane0;

Texture xTexture;
Texture xTexture0; 
Texture xTexture1; 
Texture xTexture2;
Texture xTexture3;
Texture xTexture4;
Texture xTexture5;
Texture xShadowMap;
texture Ground;

float DepthBias = 0.0001f;

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
	float4 clipDistances : TEXCOORD5;
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


float4 ComputeShadowColor(float4 worldPos, float4 Color)
{
	// Find the position of this pixel in light space
	float4 lightingPosition = mul(worldPos, xLightsWorldViewProjection);

		// Find the position in the shadow map for this pixel
		float2 ShadowTexCoord = 0.5 * lightingPosition.xy /
		lightingPosition.w + float2(0.5, 0.5);
	ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;

	// Get the current depth stored in the shadow map
	float4 shadowInfo = tex2D(ShadowMapSampler, ShadowTexCoord);
		float shadowdepth = shadowInfo.r;
	float shadowOpacity = (0.5f + 0.5f * (1 - shadowInfo.g));

	// Calculate the current pixel depth
	// The bias is used to prevent folating point errors that occur when
	// the pixel of the occluder is being drawn
	float ourdepth = (lightingPosition.z / lightingPosition.w) - DepthBias;

	// Check to see if this pixel is in front or behind the value in the shadow map
	//if (shadowdepth < ourdepth)
	//{
		// Shadow the pixel by lowering the intensity
		Color *= float4(shadowOpacity , shadowOpacity , shadowOpacity , 1);
	//};

	return Color;
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
	Output.clipDistances = dot(inPos, ClipPlane0); //MSS - Water Refactor added


	return Output;
}

MTPixelToFrame MultiTexturedPS(MTVertexToPixel PSIn)
{
	
	MTPixelToFrame Output = (MTPixelToFrame)0;

	float diffuseLightingFactor=0.3f;
	if (xEnableLighting)
	{
		
		diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal*(-1));
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

  //////////////////////////////////////////////////////////////////////////////////////


		





	


	float4 Color=(0,0,0,0);
	Color = ComputeShadowColor(mul(PSIn.Position, xWorldViewProjection), Output.Color);
	
	
	Output.Color = lerp(farColor, nearColor, blendWidth)*(diffuseLightingFactor + xAmbient);//+ lerp(nearColor2, farColor2, blendWidth)*(diffuseLightingFactor + xAmbient);

	//Output.Color += colour;
	
	Output.Color += Color;

	  if (Clipping)
	   clip(PSIn.clipDistances);  //MSS - Water Refactor added
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



//------- Constants --------

float3 xLightDirection;
float3 xCamPos;
float3 xAllowedRotDir;
int scale;




float4x4 xReflectionView;
float xWaveLength;
float xWaveHeight;
float xTime;
float xTime2;
float xWindForce;
float3 xWindDirection;

float xOvercast;

//------- Texture Samplers --------
Texture xReflectionMap;
Texture xWaterBumpMap;
Texture xRefractionMap;

sampler ReflectionSampler = sampler_state { texture = <xReflectionMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

sampler RefractionSampler = sampler_state { texture = <xRefractionMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

sampler WaterBumpMapSampler = sampler_state { texture = <xWaterBumpMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };










//------- Technique: Water --------
struct WVertexToPixel
{
	float4 Position                 : POSITION;
	float4 ReflectionMapSamplingPos    : TEXCOORD1;
	float2 BumpMapSamplingPos        : TEXCOORD2;
	float4 RefractionMapSamplingPos : TEXCOORD3;
	float4 Position3D                : TEXCOORD4;
};

struct WPixelToFrame
{
	float4 Color : COLOR0;
};

WVertexToPixel WaterVS(float4 inPos : POSITION, float2 inTex : TEXCOORD)
{
	WVertexToPixel Output = (WVertexToPixel)0;

	float4x4 preViewProjection = mul(xView, xProjection);
		float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);
		float4x4 preReflectionViewProjection = mul(xReflectionView, xProjection);
		float4x4 preWorldReflectionViewProjection = mul(xWorld, preReflectionViewProjection);

		Output.Position = mul(inPos, preWorldViewProjection);
	Output.ReflectionMapSamplingPos = mul(inPos, preWorldReflectionViewProjection);
	Output.RefractionMapSamplingPos = mul(inPos, preWorldViewProjection);
	Output.Position3D = mul(inPos, xWorld);

	float3 windDir = normalize(xWindDirection);
		float3 perpDir = cross(xWindDirection, float3(0, 1, 0));
		float ydot = dot(inTex, xWindDirection.xz);
	float xdot = dot(inTex, perpDir.xz);
	float2 moveVector = float2(xdot, ydot);
		moveVector.y += xTime*xWindForce;
	Output.BumpMapSamplingPos = moveVector / xWaveLength;


	return Output;
}

WPixelToFrame WaterPS(WVertexToPixel PSIn)
{
	WPixelToFrame Output = (WPixelToFrame)0;

	float4 bumpColor = tex2D(WaterBumpMapSampler, PSIn.BumpMapSamplingPos);
		float2 perturbation = xWaveHeight*(bumpColor.rg - 0.5f)*2.0f;

		float2 ProjectedTexCoords;
	ProjectedTexCoords.x = PSIn.ReflectionMapSamplingPos.x / PSIn.ReflectionMapSamplingPos.w / 2.0f + 0.5f;
	ProjectedTexCoords.y = -PSIn.ReflectionMapSamplingPos.y / PSIn.ReflectionMapSamplingPos.w / 2.0f + 0.5f;
	float2 perturbatedTexCoords = ProjectedTexCoords + perturbation;
		float4 reflectiveColor = tex2D(ReflectionSampler, perturbatedTexCoords);

		float2 ProjectedRefrTexCoords;
	ProjectedRefrTexCoords.x = PSIn.RefractionMapSamplingPos.x / PSIn.RefractionMapSamplingPos.w / 2.0f + 0.5f;
	ProjectedRefrTexCoords.y = -PSIn.RefractionMapSamplingPos.y / PSIn.RefractionMapSamplingPos.w / 2.0f + 0.5f;
	float2 perturbatedRefrTexCoords = ProjectedRefrTexCoords + perturbation;
		float4 refractiveColor = tex2D(RefractionSampler, perturbatedRefrTexCoords);

		float3 eyeVector = normalize(xCamPos - PSIn.Position3D);
		float3 normalVector = (bumpColor.rbg - 0.5f)*2.0f;

		float fresnelTerm = dot(eyeVector, normalVector);
	float4 combinedColor = lerp(reflectiveColor, refractiveColor, fresnelTerm);

		float4 dullColor = float4(0.3f, 0.3f, 0.5f, 0.3f);

		Output.Color = lerp(combinedColor, dullColor, 0.2f);

	float3 reflectionVector = -reflect(xLightDirection, normalVector);
		float specular = dot(normalize(reflectionVector), normalize(eyeVector));
	specular = pow(abs(specular), 256);													
	Output.Color.rgb += specular;

	return Output;
}

technique Water
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 WaterVS();
		PixelShader = compile ps_2_0 WaterPS();
	}
}


