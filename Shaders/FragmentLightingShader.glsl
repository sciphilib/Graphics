#version 330 core
in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

struct DirLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight {    
    vec3 position;
    
    float constant;
    float linear;
    float quadratic;  
 
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};  

uniform DirLight dirLight;
#define NR_POINT_LIGHTS 2
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPos;  
uniform vec3 viewPos;

vec3 CalculateDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

void main()
{

    // properties
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);
 
    // phase 1: directional lighting
    vec3 result = CalculateDirLight(dirLight, norm, viewDir);
 
    // phase 2: point lights
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalculatePointLight(pointLights[i], norm, FragPos, viewDir); 

    FragColor = vec4(result * objectColor, 1.0);

//    // ambient
//    float ambientStrength = 0.1f;
//    vec3 ambient = ambientStrength * lightColor;
//
//    // diffuse
//    vec3 norm = normalize(Normal);
//    vec3 lightDir = normalize(lightPos - FragPos);
//    float diffFactor = max(dot(norm, lightDir), 0.0f);
//    vec3 diffuse = diffFactor * lightColor;
//
//    // specular
//    float specularStrength = 0.5;
//    vec3 viewDir = normalize(viewPos - FragPos);
//    vec3 reflectDir = reflect(-lightDir, norm);  
//    float specFactor = pow(max(dot(viewDir, reflectDir), 0.0), 64);
//    vec3 specular = specularStrength * specFactor * lightColor;  
//
//
//    vec3 result = (ambient + diffuse + specular) * objectColor;
//    FragColor = vec4(result, 1.0f);
}


vec3 CalculateDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
 
    // diffuse
    float diff = max(dot(normal, lightDir), 0.0);
 
    // specular
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 64);
 
    // result
    vec3 ambient = light.ambient;
    vec3 diffuse = light.diffuse * diff;
    vec3 specular = light.specular * spec;
    return (ambient + diffuse + specular);
}

vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
 
    // diffuse
    float diff = max(dot(normal, lightDir), 0.0);
 
    // specular
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 64);
 
    // light attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
 
    // result
    vec3 ambient = light.ambient;
    vec3 diffuse = light.diffuse * diff;
    vec3 specular = light.specular * spec;
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
} 