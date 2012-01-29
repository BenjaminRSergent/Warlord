float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor;
float AmbientIntensity;

float3 CameraPosition;

float4 WarpPosition;

float FogNear = 150;
float FogFar = 200;
float4 FogColor = {0.5,0.5,0.5,1.0};

Texture BlockTexture;
sampler BlockTextureSampler = sampler_state
{
	texture = <BlockTexture>;
	magfilter = POINT;
	minfilter = POINT;
	mipfilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;	
	float2 TexCoords : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoords : TEXCOORD0;
    float3 CameraView : TEXCOORD1;
    float Distance : TEXCOORD3;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;	
    float4 worldPosition = mul(input.Position, World);
	worldPosition = worldPosition + normalize(worldPosition - WarpPosition)*length(worldPosition - WarpPosition/400);

    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.CameraView = normalize(CameraPosition - worldPosition);    
    output.TexCoords = input.TexCoords;	
	output.Distance = length(CameraPosition - worldPosition);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 texColor = tex2D(BlockTextureSampler, input.TexCoords);

	float4 ambient = AmbientIntensity * AmbientColor;	    
    float fog = 1 - (input.Distance-FogNear)/(FogFar-FogNear);
	fog = max(fog, 0);
	fog = min(fog, 1);
    float4 color =  texColor * ambient;
    
    return lerp(FogColor, color ,fog);
}

technique BlockTechnique
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}