using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{

    string PhpPath = "postgres://uofbkorn:YSfqMwgy3W8AnMOUH7InVpACNdWjFImg@mouse.db.elephantsql.com/uofbkorn";
    string ProfileName = "uofbkorn";
    string Password = "YSfqMwgy3W8AnMOUH7InVpACNdWjFImg";
    void Start()
    {
        StartCoroutine(AuthorizeSql());
    }


    private IEnumerator AuthorizeSql()
    {
        // replace php path  with  the address to the php file
        string post_url = PhpPath + "?username=" + WWW.EscapeURL(ProfileName) + "&password=" + WWW.EscapeURL(Password);

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        if (hs_post.error != null)
        {
            Debug.Log("error" + 1);
        }
        else
        {
            if (hs_post.text == "true")
            {
                //logged in..
            }
            else
            {
                //not logged in
            }
        }
    }
    // and somewhere in the code call  



    // Update is called once per frame
    void Update()
    {
        
    }
}
