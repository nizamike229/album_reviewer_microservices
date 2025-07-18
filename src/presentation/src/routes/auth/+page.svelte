<script lang="ts">
  import apiClient from "../../lib/apiClient";
  import { goto } from "$app/navigation";
  import {
    setAccessToken,
    getAccessToken,
    setUsername,
  } from "../../lib/apiClient";

  let username = "";
  let password = "";
  let error = "";
  let isLogin = true;

  async function submit() {
    error = "";
    try {
      if (isLogin) {
        const res = await apiClient.post("/api/auth/login", {
          username,
          password,
        });
        if (res.data) {
          setAccessToken(res.data);
          setUsername(username);
          goto("/");
        } else {
          error = "Ошибка входа: токен не получен";
        }
      } else {
        await apiClient.post("/api/auth/register", { username, password });
        isLogin = true;
      }
    } catch (e: any) {
      error = e?.response?.data?.message || "Ошибка запроса";
    }
  }
</script>

<div
  class="min-h-screen flex items-center justify-center bg-gradient-to-br from-zinc-900 via-indigo-950 to-zinc-900 font-mono"
>
  <div
    class="bg-zinc-900 rounded-3xl shadow-2xl p-10 w-full max-w-md border border-indigo-900"
  >
    <h1 class="text-3xl font-bold mb-6 text-center text-indigo-400">
      {isLogin ? "Вход" : "Регистрация"}
    </h1>
    <form on:submit|preventDefault={submit} class="space-y-6">
      <input
        type="text"
        bind:value={username}
        placeholder="Имя пользователя"
        class="w-full px-4 py-3 rounded-xl border border-indigo-900 bg-zinc-800 text-zinc-100 focus:ring-2 focus:ring-indigo-500 outline-none font-mono"
      />
      <input
        type="password"
        bind:value={password}
        placeholder="Пароль"
        class="w-full px-4 py-3 rounded-xl border border-indigo-900 bg-zinc-800 text-zinc-100 focus:ring-2 focus:ring-indigo-500 outline-none font-mono"
      />
      {#if error}
        <div class="text-red-400 text-center">{error}</div>
      {/if}
      <button
        type="submit"
        class="w-full py-3 rounded-xl bg-indigo-700 hover:bg-indigo-800 text-white font-bold font-mono transition"
        >{isLogin ? "Войти" : "Зарегистрироваться"}</button
      >
    </form>
    <div class="mt-6 text-center">
      <button
        class="text-indigo-400 hover:underline font-mono"
        on:click={() => (isLogin = !isLogin)}
      >
        {isLogin
          ? "Нет аккаунта? Зарегистрироваться"
          : "Уже есть аккаунт? Войти"}
      </button>
    </div>
  </div>
</div>
