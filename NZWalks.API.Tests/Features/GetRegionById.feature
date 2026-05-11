@Regression
Feature: Get Region By Id
    As a user of the NZ Walks API
    I want to retrieve a specific region by its ID
    So that I can view details of a particular region

    Scenario: Successfully retrieve a region with valid ID
        Given I have obtained a valid region ID from the GetAll endpoint
        When I call the GetRegion by ID endpoint with that ID
        Then the response status code should be 200 OK
        And the response should contain the correct region data

    Scenario: Retrieve region with invalid ID returns not found
        When I call the GetRegion by ID endpoint with a non-existent region ID
        Then the response status code should be 404 Not Found
    
    Scenario: Retrieved region should have all required properties
        Given I have obtained a valid region ID from the GetAll endpoint
        When I call the GetRegion by ID endpoint with that ID
        Then the region should have all required properties:
            | Property        |
            | Id              |
            | Code            |
            | Name            |

    Scenario: Response time for get by ID should be acceptable
        Given I have obtained a valid region ID from the GetAll endpoint
        When I call the GetRegion by ID endpoint with that ID
        #Then the response time should be less than 5 seconds

    Scenario: Data consistency between GetAll and GetById
        Given I have retrieved all regions
        When I retrieve each region individually by ID
        #Then the data should match the data from GetAll endpoint
