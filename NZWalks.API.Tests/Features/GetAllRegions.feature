@Regression
Feature: Get All Regions
    As a user of the NZ Walks API
    I want to retrieve all regions
    So that I can see the complete list of available regions

    Scenario: Successfully retrieve all regions
        When I call the GetAll regions endpoint
        Then the response status code should be 200 OK
        And the response should contain a list of regions

    Scenario: Verify each region has required properties
        When I call the GetAll regions endpoint
        Then each region in the response should have:
            | Property        |
            | Id              |
            | Code            |
            | Name            |

    Scenario: Response should contain valid Region DTOs
        When I call the GetAll regions endpoint
        Then the response should contain valid Region DTOs with all required properties

    Scenario: Response time should be acceptable
        When I call the GetAll regions endpoint
        #Then the response time should be less than 5 seconds

    Scenario: Response should have correct content type
        When I call the GetAll regions endpoint
        #Then the response header Content-Type should contain "application/json"
