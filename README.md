# Image Repository 

This is part of the my Shopify 2021 Challenge

## Set Up

### Docker [Option 1]

To create and run a docker image use this option.

* Clone this repository

```git clone https://github.com/adbrik/shopifysummer2021.git```

* Enter the root of the repository

* Build the docker image

```docker build -t "imagerepo:latest" .```

* Run the container

```docker run -d -p 8080:8080 --name imagerepo imagerepo```

* The Web App will be running on http://localhost:8080

### dotnet core runtime [Option 2]

Alternatively if dotnet core 3.1 is installed use this option.

* Clone this repository

```git clone https://github.com/adbrik/shopifysummer2021.git```

* Enter the root of the repository

* Build the project

```dotnet publish -c Release -o build```

* Run the .dll file

```dotnet /build/ShopifySummer2021.dll```

## Usage

There  are three end points:

|    Add Images |
| :------------------|
| [POST] http://localhost:8080/{bucket}/add     |

Adds images to a bucket, where bucket is a unique user (can be anything)

###### Response

The request returns a list of image ids
```
[
    "7a81050f"
]
```

###### Parameters

imageData list : A list of Base64 Encoded Images

isPrivate bool [optional] : determines if the images uploaded will be private

password string [optional] : password to view the photos uploaded

###### Example

Example body requests are provided in repository under /examples

|    Get ImageIds  |
| :------------------|
| [GET] http://localhost:8080/{bucket}     |

###### Response

The request returns a list of image ids
```
[
    "7a81050f",
    "b1f7a5f8",
    "616970a3",
    "faa2d852"
]
```

###### Parameters

###### Example

```http://localhost:8080/harley```

|    Get Image  |
| :------------------|
| [GET] http://localhost:8080/{bucket}/{imageId}     |

###### Response

The request returns an image

###### Parameters

access : password to view private image

###### Example

non private image:

```http://localhost:8080/harley/faa2d852```

private image:

```http://localhost:8080/harley/faa2d852?access=catsarecute!23```