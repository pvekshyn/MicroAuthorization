Feature: User

Scenario: User success flow
	Given I am logged in as admin

	When create user Mary with email mary@gmail.com
	Then result OK

	When get user Mary
	Then result OK

	When delete user Mary
	Then result OK

	When get user Mary
	Then result NotFound

Scenario: Not authenticated
	Given I am not logged in
	When create user Mary with email mary@gmail.com
	Then result Unauthorized

Scenario: No permission
	Given I am logged in as user without permissions
	When create user Mary with email mary@gmail.com
	Then result Forbidden

Scenario: Invalid email
	Given I am logged in as admin
	When create user Mary with email invalid
	Then result BadRequest
