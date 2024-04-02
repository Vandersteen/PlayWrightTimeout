# Note

We get timeouts intermittently when running this kind of workloads on an aks cluster;
I do not own the duende website shown here in the sample, so run this sample with care. (Do not spam them)

# Build

```
docker build -t test .
```

# Run

```
docker run -it -m="512m" --memory-swap="512m" --cpus="0.5" test
```
