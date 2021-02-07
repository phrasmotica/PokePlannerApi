// JENKINS PARAMETERS
// - BRANCH_NAME: the branch to build

node {
    dir("PokeApiNet") {
        stage("Checkout PokeApiNet") {
            git(
                url: "https://github.com/phrasmotica/PokeApiNet",
                branch: "poke-planner-web"
            )
        }

        stage("Build PokeApiNet") {
            powershell "dotnet build"
        }
    }
    
    dir("PokePlannerWeb") {
        stage("Checkout PokePlannerWeb") {
            checkout scm
        }

        stage("Build PokePlannerWeb") {
            powershell "dotnet build"
        }
    }

    stage("Clean workspace") {
        deleteDir()
    }
}