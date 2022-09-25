#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 ourColor; // output a color to the fragment shader


void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0f);
    ourColor = aColor; // set ourColor to the input color we got from the vertex data
}