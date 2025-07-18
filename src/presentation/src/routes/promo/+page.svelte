<script lang="ts">
  import { goto } from "$app/navigation";
</script>

<div
  class="min-h-screen flex items-center justify-center bg-gradient-to-br from-zinc-900 via-indigo-950 to-zinc-900 font-mono"
>
  <div
    class="bg-zinc-900 rounded-3xl shadow-2xl p-10 w-full max-w-xl text-center border border-indigo-900"
  >
    <h1 class="text-4xl font-bold mb-6 text-indigo-400">Добро пожаловать!</h1>
    <p class="text-lg text-zinc-200 mb-8">
      Чтобы пользоваться сервисом, войдите или зарегистрируйтесь.<br />Откройте
      для себя мир музыки и альбомов!
    </p>
    <button
      class="px-8 py-4 rounded-xl bg-indigo-700 hover:bg-indigo-800 text-white font-bold font-mono text-xl transition"
      on:click={() => goto("/auth")}>Войти или зарегистрироваться</button
    >
  </div>
</div>
