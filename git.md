# Git Aliases & Shortcuts (Ubuntu Terminal)

This document lists all the Git shortcuts you can use after adding them to your `~/.bashrc` or `~/.zshrc`.

---

## 🔹 Cherry-pick

### Cherry-pick a range of commits

```bash
gcpickrange commit1 commit2
```

➡️ Runs:

```bash
git cherry-pick commit1..commit2
```

---

## 🔹 Copy Changes from Another Branch

### Apply only differences (like a patch)

```bash
gcopydifference branch-name
```

➡️ Runs:

```bash
git diff branch-name | git apply
```

### Checkout everything from another branch

```bash
gcopycheckout branch-name
```

➡️ Runs:

```bash
git checkout branch-name -- .
```

### Checkout a specific file from another branch

```bash
gcopyfile branch-name path/to/file
```

➡️ Runs:

```bash
git checkout branch-name -- path/to/file
```

---

## 🔹 Restore File from Another Branch

### Restore a specific file from a branch/commit

```bash
grestore branch-name path/to/file
```

➡️ Runs:

```bash
git restore --source=branch-name path/to/file
```

---

## 🔹 Commit & Stash

### Commit with a message

```bash
gcm "Your commit message"
```

➡️ Runs:

```bash
git commit -m "Your commit message"
```

### Stash with a message

```bash
gstash "Your stash message"
```

➡️ Runs:

```bash
git stash push -m "Your stash message"
```

---
