using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class Movement
    {
        private GameObject player;

        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(player);
            SceneManager.UnloadSceneAsync("TestScene");
        }

        [UnityTest]
        public IEnumerator LeftAndRightMovementTest()
        {
            player = GameObject.Find("Player");
            Vector3 position = player.GetComponent<Rigidbody2D>().transform.position;
            player.GetComponent<Runtime2DMovement>().moveRight();
            yield return new WaitForSeconds(1.0f);
            Assert.Greater(player.GetComponent<Rigidbody2D>().position.x, position.x);

            position = player.GetComponent<Rigidbody2D>().position;
            player.GetComponent<Runtime2DMovement>().moveLeft();
            yield return new WaitForSeconds(1.0f);
            Assert.Less(player.GetComponent<Rigidbody2D>().position.x, position.x);
        }

        [UnityTest]
        public IEnumerator IntialJumpTest()
        {
            player = GameObject.Find("Player");
            Vector3 position = player.GetComponent<Rigidbody2D>().transform.position;
            player.GetComponent<Runtime2DMovement>().intialJump();
            yield return new WaitForSeconds(0.2f);
            Assert.Greater(player.GetComponent<Rigidbody2D>().position.y, position.y);
        }

        [UnityTest]
        public IEnumerator IsGroundedAfterJump()
        {
            player = GameObject.Find("Player");
            Vector3 position = player.GetComponent<Rigidbody2D>().transform.position;
            player.GetComponent<Runtime2DMovement>().intialJump();
            yield return new WaitForSeconds(1.0f);
            Assert.IsTrue(player.GetComponent<Runtime2DMovement>().getIsGrounded());
        }

        [UnityTest]
        public IEnumerator continuousJumpTest()
        {
            player = GameObject.Find("Player");
            Vector3 position = player.GetComponent<Rigidbody2D>().transform.position;
            player.GetComponent<Runtime2DMovement>().intialJump();
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(player.GetComponent<Rigidbody2D>().position.y, position.y);

            position = player.GetComponent<Rigidbody2D>().transform.position;
            player.GetComponent<Runtime2DMovement>().continuousJump();
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(player.GetComponent<Rigidbody2D>().position.y, position.y);

            position = player.GetComponent<Rigidbody2D>().transform.position;
            player.GetComponent<Runtime2DMovement>().continuousJump();
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(player.GetComponent<Rigidbody2D>().position.y, position.y);
        }
    }
}