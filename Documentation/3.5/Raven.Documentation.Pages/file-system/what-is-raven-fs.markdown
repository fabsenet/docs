#What is RavenFS?

Raven File System (RavenFS) is a distributed virtual file system integrated with RavenDB to provide first class support for binary data.
Since RavenDB 3.0 it has been a recommended way to store your binary files, instead of using the deprecated attachment mechanism.

It was designed to handle very large files (multiple GBs) efficiently at API and storage layers level by minimizing the amount of duplicated data between the files.
It has a built-in file indexing support that allows you to search files by their associated metadata (such as size of a file, a modification date or custom metadata defined by user).

RavenFS is a replicated and highly available system. It provides an optimized file synchronization mechanism, which ensures that only differences between the files are transferred over the network to synchronize it between configured nodes. This lets you update very large files and replicate only the changes - everything is transparent for the user, all you need is to specify destination nodes.

##Basic concepts

###File

An essential item that you will work with is a file. Besides binary data that makes up the file's content, each one has associated metadata. There are two kinds of metadata:

* the first one is provided by the system and internally used by it (for instance: `Etag`),
* the second one is defined by the user and can contain any information under a custom key.

As already mentioned, metadata is available for searching. You can find more details about how the files are stored internally in the [Files](files) article.

###Configuration

A configuration item is for keeping non-binary data as a collection of key/value properties stored under a unique name. Note that the configuration can be completely unrelated to your files but it can hold additional information, important for your application. It is also used internally by RavenFS to store some configuration settings (i.e. `Raven/Synchronization/Destinations` keeps addresses of the synchronization destination nodes).

###Indexing

Files are indexed by default. It allows you to execute queries against the metadata of existing files. Under the hood, just like in RavenDB, Lucene search engine is used. This allows you to make an efficient search by using the file name, size and metadata.

###Synchronization

Synchronization between RavenFS nodes works out of the box. The only thing you need to do is to provide a list of the destination file systems. 
Once one of the following happens, then it will automatically start to synchronize an affected file:

* new file uploaded,
* file content changed,
* file metadata changed.
* file renamed,
* file deleted.

The synchronization task also runs periodically to handle failures and restart scenarios. Each of the operations above is related to a different kind of synchronization work, which is determined by the server in order to minimize the amount of transferred data across the network. For example, if you change the file name alone, there is no need to send the file's content. The destination file system simply needs to know what is the new name of the file. To get more details about implemented synchronization solutions click [here](./synchronization/how-it-works).

##Management studio

You can easily manage your files by using HTML5 application studio. Databases as well as file systems are handled by the same application accessible under RavenDB server url.

![Figure 1. Studio. File system](images/studio_view.png)  
