# TerrainStudy

What it currently does:
* Instantiates a map based on source textures. Basically textures are information of where to place indexed voxel.
* Each voxel has a type and each have separated meshes for various directions.
* Instantiated map is in a layering system, so it can instantiate multiple objects on top of each other to create complex structures.
* Instantiated objects are merged into a single mesh.

TODO:

* Convert faces using Quaternions to Vector3 on the same space as texture so we can use this info to determine neighbor pixels
* Delete or do not instantiate angles that will get on top of neighboor meshes.
* We must be able to create seams between different types, so when Generator finds a type that has a valid seam already assigned, it uses it to fill the gap between multiple voxel types.
* Create seams, geometry between weldable vertices.
