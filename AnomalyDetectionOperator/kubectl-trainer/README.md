# Available Commands

To list all `anomalydetection.andreee94.ml/Trainer` resources:

```bash
kubectl trainer get 
```

```bash
kubectl trainer get -n $NAMESPACE
```

To list all `anomalydetection.andreee94.ml/Datasource` resources:

```bash
kubectl trainer get --datasource
```

```bash
kubectl trainer get -n $NAMESPACE --datasource
```

To have a yaml template of a `anomalydetection.andreee94.ml/Trainer` resource:

```bash
kubectl trainer create 
```

```bash
kubectl trainer create -n $NAMESPACE
```

To have a yaml template of a `anomalydetection.andreee94.ml/Datasource` resource:

```bash
kubectl trainer create --datasource
```

```bash
kubectl trainer create -n $NAMESPACE --datasource
```

To optimize the trainer job distribution:

```bash
kubectl trainer distribute
```
