#version 330 core
out vec4 FragColor;

float map(float value, float min1, float max1, float min2, float max2) {
  return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

void main()
{
//    map(0.3, 0.0, 1.0, 0.0, 255.0);
    FragColor = vec4(vec3(0.90f, 0.44f, 0.09f), 1.0f);
}