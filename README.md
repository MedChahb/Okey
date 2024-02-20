# Okey

[![état du pipeline](https://git.unistra.fr/okaybytes/okey/badges/main/pipeline.svg)](https://git.unistra.fr/okaybytes/okey/-/commits/main)
[![rapport de couverture](https://git.unistra.fr/okaybytes/okey/badges/main/coverage.svg)](https://git.unistra.fr/okaybytes/okey/coverage)
[![dernière version](https://git.unistra.fr/okaybytes/okey/-/badges/release.svg)](https://git.unistra.fr/okaybytes/okey/-/releases/permalink/latest)
[![pre-commit](https://img.shields.io/badge/pre--commit-enabled-brightgreen?logo=pre-commit)](https://github.com/pre-commit/pre-commit)

Vous trouverez ici le projet [Okey](https://git.unistra.fr/okaybytes/okey) qui est un jeu de table entre 2 à 4 personnes surtout connu et réparti en Turquie.

Voir la [documentation](https://okaybytes.pages.unistra.fr/okey) complète pour plus de détails sur l'utilisation et l'installation.

## Documents internes

* [Cahier des charges](https://docs.google.com/document/d/1Rsr_0wPjUDgFJGwcGmsh3udF6GZKAEvckagoF2dQsZc/edit?usp=sharing)
* [Tâches frontend](https://docs.google.com/document/d/1juRMyaspqpbGG-6astmrf9sYpfhF4I-d_2sC1IXX4jM/edit?usp=sharing)
* [Tâches backend](https://docs.google.com/document/d/1tw6rdk8RJixN4Gf4Xx8rJSAIJKp01h6838isS8QlBjY/edit?usp=sharing)
* [Team log](https://docs.google.com/spreadsheets/d/1c_RrIB0kDTPiEfFVfjjMwCOlqc8TLVP4LDvLlRVz5Ng/edit?usp=sharing) (ancien)
* [Team log](https://docs.google.com/spreadsheets/d/1Lx6XSw-6TPpPX6M-f2ByZmqWlIIDPr0a4ye2qn6C0Tg/edit?usp=sharing)
* [Spécifications backend](https://docs.google.com/document/d/138RZhz9YMOilw_RB4Bq7qBsjzFfuHsJlgB5n1W3kzYc/edit?usp=sharing)
* [Figma](https://www.figma.com/file/ZTVmviAU1qUqfNIfh04j1V/Untitled?type=design&node-id=0%3A1&mode=design&t=2wq9oDEamRp9MzS5-1)

## Environnement de développement

*Pour une utilisation en tant que développeur, un environnement dev*.

### Dépendances

* [Git](https://git-scm.com/) récent
* [Python](https://www.python.org/) >= 3.9 stable (3.10.13 recommandé)
  * [pre-commit](https://pre-commit.com/) >= 3.6.1
  * [python-gitlab](https://python-gitlab.readthedocs.io/en/v4.4.0/) >= 4.4.0
  * [python-dotenv](https://pypi.org/project/python-dotenv/) >= 1.0.1
  * [gitlint](https://jorisroovers.com/gitlint/latest/) >= 0.19.1
* [.NET Core](https://dotnet.microsoft.com/en-us/download) 8.0.1 LTS
  * [csharpier](https://csharpier.com/) >= 0.27.2
  * [docfx](https://dotnet.github.io/docfx/index.html) >= 2.75.2
  * [xunit](
  * [roslynator](
* [Unity](https://unity.com/fr) 2022.3.19f1 LTS

### Étapes d'installation

Veillez à utiliser vos identifiants GitLab Unistra dans votre dépôt local pour que les commits soient bien attribués à votre compte. Configurez aussi la signature de vos commits et tags pour garantir leur authenticité (voir [cette section](#dépôt-git)).

```sh
git clone git@git.unistra.fr:okaybytes/okey.git
cd okey
git config --local user.name "NOM PRENOM"  # utilisez votre nom sur GitLab Unistra
git config --local user.email "ernest@etu.unistra.fr"  # utilisez votre email sur GitLab Unistra
git config --local commit.gpgsign true  # active la signature des commits automatique
dotnet tool restore  # install the dotnet tools

```

Depuis le répertoire racine du projet :

Si vous voulez créer et travailler dans un environnement virtuel pour le projet (conseillé), installez et configurez [`pyenv`](https://github.com/pyenv/pyenv?tab=readme-ov-file#installation) et puis, exécutez les commandes suivantes avant la suite :

```sh
pyenv install 3.10.13
pyenv virtualenv 3.10.13 okey  # create a virtual environment
pyenv local okey  # set the virtual environment for this directory
python -m pip install --upgrade pip  # upgrade pip to the latest version
```

Ou bien vous pouvez utiliser `venv` ou `virtuelenv` pour créer et gérer l'environnement virtuel mais veillez à ne pas oublier de l'activer avant chaque fois que vous allez travailler sur le projet (dans ce cas l'activation et la désactivation n'est pas automatique).

Avant de passer à la suite, téléchargez le fichier `.env` qui contient les variables d'environnement nécessaires pour le projet depuis le [Seafile partagé](https://seafile.unistra.fr/smart-link/2ab30c65-6f80-4575-a26b-90b170481569/) Unistra et mettez le dans le répertoire racine du projet.

Ensuite, installez les dépendances du projet :

```sh
pip install -r requirements.txt  # install the pip packages
pre-commit install -t pre-commit -t pre-push -t prepare-commit-msg -t commit-msg  # install git hooks
pre-commit run -a  # run git hooks once for the first time
```

*Lire aussi les sections en dessous sur les tests unitaires et la génération de doc pour avoir votre environnement de dev complètement réglé.* (à venir ...)

### Astuce git hooks

Si à cause des git hooks, votre commit a été refusé après que vous avez renseigné votre message de commit, pas de panique c'est pas perdu, vous pouvez le récupérer et l'éditer pour le corriger en exécutant la commande suivante :

```sh
git commit -eF .git/COMMIT_EDITMSG
```

Vous pouvez aussi désactiver les git hooks pour un commit en exécutant la commande suivante :

```sh
git commit --no-verify
```

Notez que les git hooks sont là pour vous aider à écrire des commits propres et bien formatés. Essayez de n'utiliser cette astuce qu'en cas de force majeure ou si vous venez d'exécuter les git hooks et tout s'est bien passé, et vous savez que rien n'a changé entre temps, pour ne pas relancer les hooks et perdre du temps quand vous savez déjà le résultat, vous pouvez faire cela. Mais puisqu'un certain nombre de ces hooks sont aussi lancé coté serveur, si vous les désactivez vous n'aurez pas la garantie que votre commit sera accepté par le serveur lors du CI et votre merge request peut être bloqué.

### Astuce hook `gitlint`

Vous pouvez le désactiver pour un commit en mettant une des deux formules suivantes sur une nouvelle ligne (forcément au tout début de la ligne) dans votre message de commit :

* `+no-gitlint` (désactive toutes les vérifications)
* `gitlint-ignore: all` (configuration fine possible pour ne désactiver que certaines vérifications), voir les détails sur <https://jorisroovers.com/gitlint/latest/configuration/commit_config/>

Par exemple :

```git
GL-881: Fix a typo in the README

Here is a body message going into more details about the commit.

+no-gitlint

# Please enter the commit message for your changes. Lines starting
# with '#' will be ignored, and an empty message aborts the commit.
#
...
```

### Documentation

La documentation est générée par `docfx` et se trouve dans le répertoire `docs/`. Pour la générer, exécutez la commande suivante :

```sh
dotnet docfx docs/docfx.json
```

Voir <https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags> pour plus de détails sur la documentation XML C# dans le code qui ressemble un peu à `Doxygen` ou les `Docstring` Python mais un format different.

## Dépôt Git

Le dépôt git de [ce projet](https://git.unistra.fr/okaybytes/okey) suit le modèle de branchement git "[Github Flow](https://docs.github.com/en/get-started/using-github/github-flow)" avec une obligation de merge request pour chaque modification et une issue GitLab pour chaque tâche ou problème. La branche `main` est protégé et ne peut être modifié que par des merge request validées par le CI. Les merge request doivent être validées par au moins un autre développeur avant d'être fusionnées.

Les noms des branches doivent avoir la référence vers l'issue correspondante avec le format `<numéro de issue>-<titre-de-la-branche>` (par exemple `42-add-a-new-feature`). Chaque commit doit être lié à une issue avec le format `GL-<numéro de issue>: <reste du titre>` au tout début du message de commit. Normalement, si les hooks git sont bien configurés dans votre environnement de développement local, ces vérifications seront faites automatiquement à chaque commit et la référence d'issue dans le titre de chaque commit automatiquement inséré et vérifié pour vous.

Pour suivre ce modèle, suivez les étapes suivantes :

* Créez une issue sur GitLab pour votre tâche ou votre problème en utilisant bien les bons tags et le bon milestone. Assurez vous de ne pas créer une issue dupliqué en vérifiant si une telle issue n'existe pas encore.
  * Mettez le label `Doing` tant que vous travaillez sur la tâche mentionnée dans l'issue. Enlevez le label quand vous avez fini.
* Créez une branche et une merge request à partir de l'issue sur GitLab, puis récupérez cette branche avec `git fetch` et `git merge` ou `git pull`. Ou bien créez votre branche en locale en veillant au bon format de nom de branche et poussez la sur le dépôt distant et créez une merge request à partir de la branche sur GitLab. La merge request est l'endroit où la discussion sur le code se passe, donc il est mieux de la créer dès le départ pour faciliter la collaboration même si votre solution n'est pas encore prête ou vous n'avez rien à montrer encore.
  * Veillez à bien lier la merge request à l'issue correspondante.
* Une fois votre merge request est prête, demandez à un autre développeur de la relire et de la valider. Si tout est bon, elle sera fusionnée avec la branche `main` et la branche de travail sera supprimée.
  * Mettez à jour le fichier `CHANGELOG.md` avec les changements apportés au fur et à mesure.
* Quand on arrive à une version stable (les jalons), on crée une release avec un tag correspondant et on met à jour le fichier `CHANGELOG.md` avec les changements apportés. L'intégration continue (CI) se chargera de publier la nouvelle version sur le serveur de production.

Tout le développement se passe sur des branches tiers. Une fois une tâche ou fonctionnalité est complété, elle est testé, et sa merge request est fusionnée avec la branche `main`.

Dernièrement, la signature de vos commits et tags est obligatoire. Voir les pages suivantes pour apprendre à comment le faire :

* <https://docs.gitlab.com/ee/user/project/repository/signed_commits/>
* <https://git-scm.com/book/en/v2/Git-Tools-Signing-Your-Work>
