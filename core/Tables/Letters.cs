using Gaucho;
using System.Collections.Generic;

public class Letters
{
 // Gambas class file

 // Las letras LFF se dibujan como una LWPOLYLINE, con trazos y bulges.
public int Code ;         
public List<List<double>> FontGlyps ;          // las lineas que dibujan uns letra
public List<List<double>> FontBulges ;          // los semicirculos que forman la letra

public Letters()
{
    FontGlyps = new List<List<double>>();
    FontBulges = new List<List<double>>();
}

}