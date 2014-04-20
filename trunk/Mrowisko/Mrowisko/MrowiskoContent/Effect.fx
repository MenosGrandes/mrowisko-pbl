
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




float4x4 xReflectionView;
float xWaveLength;
float xWaveHeight;
float xTime;
float xWindForce;
float3 xWindDirection;

float xOvercast;

bool Clipping;
float4 ClipPlane0;
//------- Texture Samplers --------
Texture xTexture;
Texture xBillboardTexture;

sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; }; Texture xTexture0;

sampler TextureSampler0 = sampler_state { texture = <xTexture0> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};Texture xTexture1;

sampler TextureSampler1 = sampler_state { texture = <xTexture1> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};Texture xTexture2;

sampler TextureSampler2 = sampler_state { texture = <xTexture2>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; }; Texture xTexture3;

sampler TextureSampler3 = sampler_state { texture = <xTexture3>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

sampler textureSampler = sampler_state { texture = <xBillboardTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; }; Texture xReflectionMap;

sampler ReflectionSampler = sampler_state { texture = <xReflectionMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = clamp; AddressV = clamp; }; Texture xRefractionMap;

sampler RefractionSampler = sampler_state { texture = <xRefractionMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = clamp; AddressV = clamp; }; Texture xWaterBumpMap;

sampler WaterBumpMapSampler = sampler_state { texture = <xWaterBumpMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

//------- Technique: Textured --------
struct TVertexToPixel
{
float4 Position     : POSITION;
float4 Color        : COLOR0;
float LightingFactor: TEXCOORD0;
float2 TextureCoords: TEXCOORD1;
float2 clipDistances: TEXCOORD2;
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
	float4x4 preViewProjection = mul(xView, xProjection);
		float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);

		Output.Position = mul(inPos, preWorldViewProjection);
	Output.TextureCoords = inTexCoords;

	float3 Normal = normalize(mul(normalize(inNormal), xWorld));
		Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = saturate(dot(Normal, -xLightDirection));

	Output.clipDistances = dot(inPos, ClipPlane0); //MSS - Water Refactor added

	return Output;
}

TPixelToFrame TexturedPS(TVertexToPixel PSIn)
{
	TPixelToFrame Output = (TPixelToFrame)0;

	Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
	Output.Color.rgb *= saturate(PSIn.LightingFactor + xAmbient);

 if (Clipping)  clip(PSIn.clipDistances);  //MSS - Water Refactor added


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
	 float4 clipDistances                : TEXCOORD5;

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

		finalPosition += ((inTexCoord.x - 0.5f)*(scale/30))*sideVector*scale;
	finalPosition += ((1.5f - inTexCoord.y*1.5f)*(scale/30))*upVector*scale;


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

	float4 bumpColor = tex2D(	WaterBumpMapSampler, PSIn.BumpMapSamplingPos);
		float2 perturbation = xWaveHeight*(bumpColor.rgb - 0.5f)*2.0f;

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

		

	float4 dullColor = float4(0.3f, 0.3f, 0.5f, 1.0f);
		Output.Color = dullColor;
			   Output.Color = lerp(combinedColor, dullColor, 0.2f);

		float3 reflectionVector = -reflect(xLightDirection, normalVector);
		float specular = dot(normalize(reflectionVector), normalize(eyeVector));
	specular = pow(abs(specular), 131);
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


