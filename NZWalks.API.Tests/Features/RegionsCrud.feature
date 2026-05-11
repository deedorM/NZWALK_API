@Regression
Feature: Regions CRUD Operations
    As an administrator of the NZ Walks API
    I want to perform CRUD operations on regions
    So that I can manage the regions in the system

    @ignore
    Scenario: Complete CRUD workflow - Create, Read, Update, Delete
        Given I have a new region to create with:
            | Code   | TestRGN |
            | Name   | Test Region |
            | ImageUrl | https://example.com/test.jpg |
        When I create the region
        Then the response status code should be 201 Created
        And the created region should be returned with a valid ID
        When I retrieve the created region by ID
        Then the region data should match the created data
        When I update the region with:
            | Code   | UPDATED |
            | Name   | Updated Region Name |
            | ImageUrl | https://example.com/updated.jpg |
        Then the update response should be successful
        And the retrieved region should reflect the updated data
        When I delete the region
        Then the delete response should be successful
    
    @ignore
    Scenario: Create region returns 201 Created
        Given I have a new region to create with:
            | Code   | NZ001 |
            | Name   | Test Region One |
            | ImageUrl | https://example.com/region1.jpg |
        When I create the region
        Then the response status code should be 201 Created

    Scenario: Concurrent requests should all succeed
        When I make 5 concurrent requests to get all regions
        Then all concurrent requests should succeed
        And all responses should contain regions

    Scenario: API handles edge cases properly
        When I call the GetAll regions endpoint
        Then the API should handle the response gracefully
        And the response should contain a valid list

    Scenario: Handle data consistency across endpoints
        Given I have retrieved all regions
        When I query each region individually
        Then each region from GetAll should match its individual GetById response
