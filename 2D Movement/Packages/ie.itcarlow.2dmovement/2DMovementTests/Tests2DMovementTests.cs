﻿using System.Collections;
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
            MovingStateMachine msm = player.GetComponent<MovingStateMachine>();
            player.GetComponent<MovingStateMachine>().setInitalState(player.GetComponent<MovingStateMachine>().movementRight);
            player.GetComponent<MovingStateMachine>().movementRight.moveRight();
            yield return new WaitForSeconds(1.0f);
            Assert.Greater(player.GetComponent<Runtime2DMovement>().getRigidBody().transform.position.x, position.x);

            position = player.GetComponent<Rigidbody2D>().transform.position;
            player.GetComponent<MovingStateMachine>().setInitalState(player.GetComponent<MovingStateMachine>().movementLeft);
            player.GetComponent<MovingStateMachine>().movementLeft.moveLeft();
            yield return new WaitForSeconds(1.0f);
            Assert.Less(player.GetComponent<Rigidbody2D>().position.x, position.x);
        }

        [UnityTest]
        public IEnumerator IntialJumpTest()
        {
            player = GameObject.Find("Player");
            Vector3 position = player.GetComponent<Rigidbody2D>().transform.position;
            MovingStateMachine msm = player.GetComponent<MovingStateMachine>();
            player.GetComponent<MovingStateMachine>().setInitalState(player.GetComponent<MovingStateMachine>().jumping);
            player.GetComponent<MovingStateMachine>().jumping.handleJumpInput();
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(player.GetComponent<Rigidbody2D>().position.y, position.y);
        }

        [UnityTest]
        public IEnumerator IsGroundedAfterJump()
        {
            player = GameObject.Find("Player");
            Vector3 position = player.GetComponent<Rigidbody2D>().transform.position;
            MovingStateMachine msm = player.GetComponent<MovingStateMachine>();
            player.GetComponent<MovingStateMachine>().setInitalState(player.GetComponent<MovingStateMachine>().jumping);
            player.GetComponent<MovingStateMachine>().jumping.handleJumpInput();
            yield return new WaitForSeconds(1.0f);
            Assert.IsTrue(player.GetComponent<Runtime2DMovement>().getIsGrounded());
        }

        [UnityTest]
        public IEnumerator continuousJumpTest()
        {
            player = GameObject.Find("Player");
            Vector3 position = player.GetComponent<Rigidbody2D>().transform.position;
            MovingStateMachine msm = player.GetComponent<MovingStateMachine>();
            yield return new WaitForSeconds(0.5f);
            player.GetComponent<MovingStateMachine>().setInitalState(player.GetComponent<MovingStateMachine>().jumping);
            player.GetComponent<MovingStateMachine>().jumping.handleJumpInput();
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(player.GetComponent<Rigidbody2D>().position.y, position.y);

            position = player.GetComponent<Rigidbody2D>().transform.position;
            player.GetComponent<Runtime2DMovement>().setJumpTimeCounter(10);
            player.GetComponent<MovingStateMachine>().jumping.continuousJump();
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(player.GetComponent<Rigidbody2D>().position.y, position.y);
        }

        [UnityTest]
        public IEnumerator healthDamageTest()
        {
            player = GameObject.Find("Player");
            int health = player.GetComponent<Runtime2DMovement>()._health;
            player.GetComponent<Runtime2DMovement>().decreaseHealth(2);
            yield return new WaitForSeconds(0.01f);
            Assert.Less(player.GetComponent<Runtime2DMovement>()._health, health);
        }

        [UnityTest]
        public IEnumerator healthAdditionTest()
        {
            player = GameObject.Find("Player");
            int health = player.GetComponent<Runtime2DMovement>()._health;
            player.GetComponent<Runtime2DMovement>().addHealth(2);
            yield return new WaitForSeconds(0.01f);
            Assert.Greater(player.GetComponent<Runtime2DMovement>()._health, health);
        }

        [UnityTest]
        public IEnumerator healthInvinibilityFramesTest()
        {
            player = GameObject.Find("Player");
            int health = player.GetComponent<Runtime2DMovement>()._health;
            player.GetComponent<Runtime2DMovement>().decreaseHealth(2);
            yield return new WaitForSeconds(0.01f);
            Assert.IsTrue(player.GetComponent<Runtime2DMovement>()._invincible);
            yield return new WaitForSeconds(3.0f);
            Assert.IsFalse(player.GetComponent<Runtime2DMovement>()._invincible);
        }


        [UnityTest]
        public IEnumerator IdleAnimationTest()
        {
            player = GameObject.Find("Player");
            Animator animator = player.GetComponent<Animator>();
            yield return new WaitForSeconds(0.01f);
            Assert.IsTrue(animator.GetBool("Idle"));
        }

        [UnityTest]
        public IEnumerator WalkingRightAnimationTest()
        {
            player = GameObject.Find("Player");
            Animator animator = player.GetComponent<Animator>();
            MovingStateMachine msm = player.GetComponent<MovingStateMachine>();
            player.GetComponent<MovingStateMachine>().setInitalState(player.GetComponent<MovingStateMachine>().movementRight);
            player.GetComponent<MovingStateMachine>().movementRight.Enter();
            player.GetComponent<MovingStateMachine>().movementRight.moveRight();
            yield return new WaitForSeconds(0.01f);
            Assert.IsTrue(animator.GetBool("WalkingRight"));
        }

        [UnityTest]
        public IEnumerator WalkingLeftAnimationTest()
        {
            player = GameObject.Find("Player");
            Animator animator = player.GetComponent<Animator>();
            MovingStateMachine msm = player.GetComponent<MovingStateMachine>();
            player.GetComponent<MovingStateMachine>().setInitalState(player.GetComponent<MovingStateMachine>().movementLeft);
            player.GetComponent<MovingStateMachine>().movementLeft.Enter();
            player.GetComponent<MovingStateMachine>().movementLeft.moveLeft();
            yield return new WaitForSeconds(0.01f);
            Assert.IsTrue(animator.GetBool("WalkingLeft"));
        }

        [UnityTest]
        public IEnumerator JumpingAnimationTest()
        {
            player = GameObject.Find("Player");
            Animator animator = player.GetComponent<Animator>();
            MovingStateMachine msm = player.GetComponent<MovingStateMachine>();
            player.GetComponent<MovingStateMachine>().setInitalState(player.GetComponent<MovingStateMachine>().jumping);
            player.GetComponent<MovingStateMachine>().jumping.Enter();
            player.GetComponent<MovingStateMachine>().jumping.handleJumpInput();
            yield return new WaitForSeconds(0.01f);
            Assert.IsTrue(animator.GetBool("Jumping"));
        }

        [UnityTest]
        public IEnumerator InvinibilityStateTest()
        {
            player = GameObject.Find("Player");
            int health = player.GetComponent<Runtime2DMovement>()._health;
            player.GetComponent<Runtime2DMovement>().decreaseHealth(2);
            yield return new WaitForSeconds(0.01f);
            Assert.IsTrue(player.GetComponent<Runtime2DMovement>()._invincible);
        }

        [UnityTest]
        public IEnumerator InvinibilityStateIsFinishedTest()
        {
            player = GameObject.Find("Player");
            int health = player.GetComponent<Runtime2DMovement>()._health;
            player.GetComponent<Runtime2DMovement>().decreaseHealth(2);
            yield return new WaitForSeconds(0.01f);
            Assert.IsTrue(player.GetComponent<Runtime2DMovement>()._invincible);
            yield return new WaitForSeconds(3.0f);
            Assert.IsFalse(player.GetComponent<Runtime2DMovement>()._invincible);
        }

        [UnityTest]
        public IEnumerator RendererIsEnalbedAfterLeavingInvinibilityState()
        {
            player = GameObject.Find("Player");
            int health = player.GetComponent<Runtime2DMovement>()._health;
            player.GetComponent<Runtime2DMovement>().decreaseHealth(2);
            yield return new WaitForSeconds(3.0f);
            Assert.IsTrue(player.GetComponent<Renderer>().enabled);
        }
    }
}