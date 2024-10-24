@regression @AdministratorAccount
Feature: Administrator Account

Background:
	Given I am logged in as administrator

@tcAa01
Scenario: 01 Tab home
	Given Be at Home tab
	Then Page must load correctly
    
@tcAa02
Scenario: 02 Create an account with out data
	Given Be at Cadastrar Usuários tab
	When Do the registration
	Then The message alert register on Register Users
    
@tcAa03
Scenario: 03 Create a regular account
	Given Be at Cadastrar Usuários tab
	When Complete Regular registration
	And Do the registration
	Then Regular registration must be on the list
   
@tcAa04
Scenario: 04 Create an account that already exists
	Given Be at Cadastrar Usuários tab
	When Complete Regular registration
	And Do the registration
	Then Regular registration must be on the list
	Given Be at Cadastrar Usuários tab
	When Complete registration already exists
	And Do the registration
	Then The message stating that this account already exists on Register Users
    
@tcAa05
Scenario: 05 Create an administrator account
  
	Given Be at Cadastrar Usuários tab
	When Complete Administrator registration
	And Do the registration
	Then Administrator registration must be on the list
    
#Listar Usuários tab----------------------------------------------------------------------

@tcAa06
Scenario: 06 Edit an account
	
	Given Be at Cadastrar Usuários tab
    When Complete Regular registration
    And Do the registration
    And Regular registration must be on the list
    When Edit account
    Then Account must be edited
    
@tcAa07
Scenario: 07 Delete an account
    
	Given Be at Cadastrar Usuários tab
	When Complete Regular registration
	And Do the registration
	And Regular registration must be on the list
	When Delete account
	Then Account must be deleted
    
#Cadastrar Produtos tab----------------------------------------------------------------------

@tcAa08
Scenario: 08 Create a product with no date
	
	Given Be at Cadastrar Produtos tab
	When Click button register
	Then Alert message should appear
    
@tcAa09
Scenario: 09 Register a product with a zero price
    
	Given Be at Cadastrar Produtos tab
	When Fill product information with zero price
	And Click button register
	Then Message price must be positive should be visible
    
@tcAa10
Scenario: 10 Register a product with a negative quantity
  
	Given Be at Cadastrar Produtos tab
	When Fill product information with negative quantity
	And Click button register
	Then Message quantity must be more or equal than zero should be visible
    
@tcAa11
Scenario: 11 Register a product
    
	Given Be at Cadastrar Produtos tab
	When Fill in product information
	And Click button register
	Then Product must be on the list
    
@tcAa12
Scenario: 12 Register a product with no quantity
    
	Given Be at Cadastrar Produtos tab
	When Fill in product no quantity
	And Click button register
	Then Product must be on the list
    
@tcAa13
Scenario: 13 Register a product with no picture
    
	Given Be at Cadastrar Produtos tab
	When Fill in product no picture
	And Click button register
	Then Product must be on the list
    
@tcAa14
Scenario: 14 Register a product that already exists
    
	Given Be at Cadastrar Produtos tab
	When Fill in product no quantity
	And Click button register
	Then Message that the product already exists should appear
    
@tcAa15
Scenario: 15 Edit a product
    
	Given Be at Listar Produtos tab
	When Find product No picture
	And Edit the item No picture
	Then Product should be edited
    
@tcAa16
Scenario: 16 Delete a product
	Given Be at Listar Produtos tab
	When Find product No picture
	And Delete the item No picture
	Then Product should be deleted