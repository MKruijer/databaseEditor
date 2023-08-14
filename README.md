# DatabaseEditor

This tool is designed to update the database and use it to finalize the cosine similarity iterations. It is build upon the database created by Bjorn et al, the paper can be found Here: https://julianpasveer.com/Relate_architectural_issues_and_emails_in_mailing_lists%20(2).pdf


## Dependency
This code is build upon the results we get using the python scripts that can be found here:
https://github.com/MKruijer/PythonSimilarityCode
We did run the scripts multiple times for different iterations, so changing the table names is advisable.


## Before running
Before running the program, make sure that you update the ConnectionString in Program.cs. It should point to a database with the following tables:
- data_email_email (contains email data)
- data_jira_jira_issue (contains issue data)
- data_jira_jira_issue_comment (contains issue comments data)
- result_arch_emails_all_issues (contains TF-IDF cosine results)
- result_arch_issues_all_emails (contains TF-IDF cosine results)
- sim_result_arch_emails_all_issues (contains SBERT cosine results)
- sim_result_arch_issues_all_emails (contains SBERT cosine results)

For iteration 4 the following tables are also required since there are new cosine values:
- iter4_sen_sim_result_arch_issues_all_emails (contains SBERT cosine results)
- iter4_cos_sim_result_arch_issues_all_emails(contains SBERT cosine results)

If you got the data_email_email table from the abovely mentioned paper, you can include the ADD type to the table by applying the script which can be found in AddADDTypeToEmails.txt 