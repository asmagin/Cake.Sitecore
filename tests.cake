#load "./content/scripts/coverage/coverage.cake"
#load "./content/scripts/git/git.cake"
#load "./content/scripts/ship/ship.cake"
#load "./content/scripts/unicorn/unicorn.cake"

#load "./content/scripts/utils.cake"

#load "./content/constants.cake"
#load "./content/parameters.cake"
#load "./content/tasks.build.cake"
#load "./content/tasks.cake"
#load "./content/tasks.clean.cake"
#load "./content/tasks.packages.cake"
#load "./content/tasks.prepare.cake"
#load "./content/tasks.publish.cake"
#load "./content/tasks.restore.cake"
#load "./content/tasks.tests.unit.cake"
#load "./content/tasks.unicorn.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = ArgumentOrEnvironmentVariable("target", "", "Default");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);