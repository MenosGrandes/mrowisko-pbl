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

float DepthBias = 1.8f;
float2 PCFSamples[17];
bool Clipping;
float4 ClipPlane0;

Texture xTexture;
Texture xTexture0; 
Texture xTexture1; 
Texture xTexture2;
Texture xTexture3;
Texture xTexture4;
Texture xTexture5;
Texture xTexture6;
Texture xTexture7;
Texture xTexture8;
Texture xTexture9;
Texture xTexture10;
Texture xTexture11;
Texture xTexture12;
Texture xTexture13;
Texture xTexture14;
Texture xShadowMap;
texture Ground;

texture shadowTexture;
sampler ShadowSampler = sampler_state
{
	Texture = (shadowTexture);
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Clamp;
	AddressV = Clamp;
};


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
sampler TextureSampler6 = sampler_state
{
	texture = <xTexture6>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};
sampler TextureSampler7 = sampler_state
{
	texture = <xTexture7>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};
sampler TextureSampler8 = sampler_state
{
	texture = <xTexture8>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};
sampler TextureSampler9 = sampler_state
{
	texture = <xTexture9>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};
sampler TextureSampler10 = sampler_state
{
	texture = <xTexture10>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};
sampler TextureSampler11 = sampler_state
{
	texture = <xTexture11>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};
sampler TextureSampler12 = sampler_state
{
	texture = <xTexture12>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};
sampler TextureSampler13 = sampler_state
{
	texture = <xTexture13>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};
sampler TextureSampler14 = sampler_state
{
	texture = <xTexture14>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
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
	float4 Pos2DAsSeenByLight    : TEXCOORD7;
	
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

		Output.Position = mul(inPos, xWorldViewProjection);
	Output.Pos2DAsSeenByLight = mul(inPos, xLightsWorldViewProjection);
	Output.Normal = mul(normalize(inNormal), xWorld);
	Output.TextureCoords = inTexCoords;
  
	Output.TextureWeights = inTexWeights;

	Output.Position3D = mul(inPos, xWorld);

	Output.Depth = Output.Position.z / Output.Position.w;
	Output.clipDistances = dot(inPos, ClipPlane0); //MSS - Water Refactor added

	return Output;
}

MTPixelToFrame MultiTexturedPS(MTVertexToPixel PSIn)
{
	
	MTPixelToFrame Output = (MTPixelToFrame)0;

	float diffuseLightingFactor2=0.3f;
	if (xEnableLighting)
	{
		
		diffuseLightingFactor2 = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal*(-1));
		diffuseLightingFactor2 = saturate(diffuseLightingFactor2);
		diffuseLightingFactor2 *= xLightPower;
	}

	float blendDistance = 0.99f;
	float blendWidth = 0.005f;
	float blendFactor = clamp((PSIn.Depth - blendDistance) / blendWidth, 0, 1);

	//Mapy alphy
	float alpha = tex2D(TextureSampler0, PSIn.TextureCoords / 200).r;
	float alpha2 = tex2D(TextureSampler1, PSIn.TextureCoords / 200).r;
	float alpha3 = tex2D(TextureSampler2, PSIn.TextureCoords / 200).r;
	float alpha4 = tex2D(TextureSampler3, PSIn.TextureCoords / 200).r;
	float alpha5 = tex2D(TextureSampler4, PSIn.TextureCoords / 200).r;
	float alpha6 = tex2D(TextureSampler5, PSIn.TextureCoords / 200).r;
	float alpha7 = tex2D(TextureSampler6, PSIn.TextureCoords / 200).r;

	//Tekstura 1
	float3 texture1 = tex2D(TextureSampler7, PSIn.TextureCoords).rgb;
		float3 texture2 = tex2D(TextureSampler8, PSIn.TextureCoords/100).rgb;
		float3 color = lerp(texture1, texture2, alpha);
		//Tekstura 2	
		float3 texture3 = tex2D(TextureSampler9, PSIn.TextureCoords / 100).rgb;
		float3 color2 = lerp(color, texture3, alpha2);
		//Tekstura 3	
		float3 texture4 = tex2D(TextureSampler10, PSIn.TextureCoords / 100).rgb;
		float3 color3 = lerp(color2, texture4, alpha3);
		//Tekstura 4	
		float3 texture5 = tex2D(TextureSampler11, PSIn.TextureCoords / 100).rgb;
		float3 color4 = lerp(color3, texture5, alpha4);
		//Tekstura 5	
		float3 texture6 = tex2D(TextureSampler12, PSIn.TextureCoords / 100).rgb;
		float3 color5 = lerp(color4, texture6, alpha5);
		//Tekstura 6	
		float3 texture7 = tex2D(TextureSampler13, PSIn.TextureCoords / 100).rgb;
		float3 color6 = lerp(color5, texture7, alpha6);
		//Tekstura 7	
		float3 texture8 = tex2D(TextureSampler14, PSIn.TextureCoords / 100).rgb;
		float3 color7 = lerp(color6, texture8, alpha7);


	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = PSIn.Pos2DAsSeenByLight.x / PSIn.Pos2DAsSeenByLight.w/ 2.0f + 0.5f;
	ProjectedTexCoords[1] = -PSIn.Pos2DAsSeenByLight.y / PSIn.Pos2DAsSeenByLight.w/ 2.0f + 0.5f;

	float depthStoredInShadowMap = tex2D(ShadowSampler, ProjectedTexCoords).r;
	float realDistance = PSIn.Pos2DAsSeenByLight.z / PSIn.Pos2DAsSeenByLight.w;
	Output.Color = float4(color7, 1)*(diffuseLightingFactor2 + xAmbient);

		for (int i = 0; i < 17; i++)
		{
			if ((realDistance - DepthBias) <= depthStoredInShadowMap)
			{

				Output.Color -= tex2D(ShadowSampler, ProjectedTexCoords + PCFSamples[i]) / 100;


		}
	}
	
		//Output.Color += tex2Dproj(TextureSampler14, PSIn.TextureWeights);
	  if (Clipping)
	   clip(PSIn.clipDistances);  //MSS - Water Refactor added
	return Output;
}

technique MultiTextured
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 MultiTexturedVS();
		PixelShader = compile ps_3_0 MultiTexturedPS();
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

		float4 dullColor = float4(0.3f, 0.3f, 0.8f, 0.9f);

		Output.Color = lerp(combinedColor, dullColor, 0.4f);

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


