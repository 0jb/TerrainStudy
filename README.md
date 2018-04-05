# TerrainStudy

What it currently does:
* Instantiates a map based on source textures. Basically textures are information of where to place indexed voxel.
* Each voxel has a type and each have separated meshes for various directions.
* Instantiated map is in a layering system, so it can instantiate multiple objects on top of each other to create complex structures.
* Instantiated objects are merged into a single mesh.
* Voxels do not have meshes overlapping between them: If a voxel has the same type as his neighbor, we discard unnecessary meshes. 

TODO:

* Make it work with Substance Designer.
* We must be able to create seams between different types, so when Generator finds a type that has a valid seam already assigned, it uses it to fill the gap between multiple voxel types.
* Detect the limit of 64k vertices and combine on multiple meshes since Unity doesn't allows us to have meshes with more than 64k vertices. Maybe this info can be baked into each voxel so generator can handle this without much cost.
