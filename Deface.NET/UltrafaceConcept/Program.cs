using Deface.NET.TestResources;
using UltrafaceConcept;

var ultraface = new Ultraface();

var image = TestResources.Photo1;

var faces = ultraface.Process(image);

foreach  (var face in faces)
{
    Console.WriteLine(face);
}