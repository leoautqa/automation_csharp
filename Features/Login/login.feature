@regression @login
Feature: Login

@log01
Scenario: 01 Login with out E-mail and password
    Given Website loaded
    When Submit login
    Then Mandatory email and password messages

@log02
Scenario: 02 Login with invalid E-mail and password
    Given Website loaded
    When Report invalid data
    And Submit login
    Then Invalid email and password message
    
 @log03
 Scenario: 03 Create an account with out data
    Given Website loaded
    When Registering a new profile
    And Send registration
    Then Mandatory message
    
@log04
Scenario: 04 Create an account
    Given Website loaded
    When Registering a new profile
    And Fill in valid data
    When Send registration
    Then Successful registration message
  
@log05
Scenario: 05 Create an account that already exists
    Given Website loaded
    When Registering a new profile
    And Fill in data that already exists
    When Send registration
    Then The message this account already exists
    
@log06
Scenario: 06 Create an administrator account
    Given Website loaded
    When Registering a new profile
    And Filling an administrator profile
    When Send registration
    Then Successful registration message