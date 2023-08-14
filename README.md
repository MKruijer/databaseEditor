# DatabaseEditor

This tool is designed to update the database and use it to finalize the cosine similarity iterations. It is build upon the database created by Bjorn et al, the paper can be found Here: https://julianpasveer.com/Relate_architectural_issues_and_emails_in_mailing_lists%20(2).pdf


## Dependency
This code is build upon the results we get using the python scripts that can be found here:
https://github.com/MKruijer/PythonSimilarityCode


## Before running
Before running the program, make sure that you update the ConnectionString in Program.cs. It should point to a database with the following tables:
- data_email_email
- data_jira_jira_issue
- data_jira_jira_issue_comment
- result_arch_emails_all_issues
- result_arch_issues_all_emails
- sim_result_arch_emails_all_issues
- sim_result_arch_issues_all_emails

If you got the data_email_email table from the abovely mentioned paper, apply the script which can be found in AddADDTypeToEmails.txt 